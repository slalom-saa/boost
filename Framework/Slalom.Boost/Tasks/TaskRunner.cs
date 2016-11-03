using System;
using Slalom.Boost.Aspects;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Tasks
{
    /// <summary>
    /// The task runner finds all scheduled tasks and executes them according to their schedules.
    /// </summary>
    public class TaskRunner
    {
        private readonly IContainer _container;
        private IScheduledTaskStore _taskStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRunner" /> class.
        /// </summary>
        /// <param name="container">The current <see cref="IContainer" /> instance.</param>
        /// <param name="taskStore">The current <see cref="IScheduledTaskStore" /> instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="taskStore" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="container" /> argument is null.</exception>
        public TaskRunner(IContainer container, IScheduledTaskStore taskStore)
        {
            Argument.NotNull(() => taskStore);
            Argument.NotNull(() => container);

            _container = container;
            _taskStore = taskStore;
        }

        /// <summary>
        /// Executes all found tasks and then updates them if they were successful.
        /// </summary>
        public void Execute()
        {
            var tasks = _taskStore.Find();

            foreach (var task in tasks)
            {
                if (this.ShouldExecute(task))
                {
                    try
                    {
                        var result = task.Execute(_container);
                        if (result.Successful)
                        {
                            _taskStore.Update(task);
                        }
                    }
                    catch (Exception exception)
                    {
                        foreach (var item in _container.ResolveAll<ILogger>())
                        {
                            item.Error($"An exception occurred while executing the {task.Name} ({task.Id}) task.", exception);
                        }
                    }
                }
            }
        }

        private bool ShouldExecute(ScheduledTask task)
        {
            switch (task.Schedule.Frequency)
            {
                case TaskFrequency.Manual:
                    return false;
                case TaskFrequency.Seconds:
                    return task.LastRun.AddSeconds(task.Schedule.Interval) < DateTimeOffset.Now;
                case TaskFrequency.Day:
                    return task.LastRun.AddDays(task.Schedule.Interval) < DateTimeOffset.Now;
                case TaskFrequency.Hour:
                    return task.LastRun.AddHours(task.Schedule.Interval) < DateTimeOffset.Now;
                case TaskFrequency.Minute:
                    return task.LastRun.AddMinutes(task.Schedule.Interval) < DateTimeOffset.Now;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}