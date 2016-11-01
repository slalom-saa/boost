using System;
using Newtonsoft.Json;
using Slalom.Boost.Commands;
using Slalom.Boost.Domain;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Tasks
{
    /// <summary>
    /// Represents a scheduled task that executes a command according to a schedule.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Domain.Entity" />
    /// <seealso cref="Slalom.Boost.Domain.IAggregateRoot" />
    public class ScheduledTask : Entity
    {
        private string _commandType;

        /// <summary>
        /// Gets the serialized command to execute.
        /// </summary>
        /// <value>The serialized command to execute.</value>
        public string Command { get; private set; }

        /// <summary>
        /// Gets the date and time that the task was created.
        /// </summary>
        /// <value>The date and time that the task was created.</value>
        public DateTimeOffset Created { get; private set; } = DateTimeOffset.Now;

        /// <summary>
        /// Gets the date and time that the task was last run.
        /// </summary>
        /// <value>The date and time that the task was last run.</value>
        public DateTimeOffset LastRun { get; private set; }

        /// <summary>
        /// Gets the name of the task.
        /// </summary>
        /// <value>The name of the task.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        /// <value>The schedule.</value>
        public TaskSchedule Schedule { get; set; }

        /// <summary>
        /// Creates a manually executed task with the specified name.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>Returns the created task.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="name"/> argument is null or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="command"/> argument is null.</exception>
        public static ScheduledTask Create(string name, ICommand command)
        {
            return Create(name, command, TaskFrequency.Manual, 0);
        }

        /// <summary>
        /// Creates a scheduled task with the specified name.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="frequency">The schedule frequency.</param>
        /// <param name="interval">The frequency interval.</param>
        /// <returns>Returns the created task.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="name"/> argument is null or whitespace.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="command"/> argument is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="frequency"/> argument is null less than 1.</exception>
        public static ScheduledTask Create(string name, ICommand command, TaskFrequency frequency, int interval)
        {
            Argument.NotNullOrWhiteSpace(() => name);
            Argument.NotNull(() => command);
            if (frequency != TaskFrequency.Manual)
            {
                Argument.GreaterThan(() => interval, 0);
            }

            return new ScheduledTask
            {
                Name = name,
                Command = JsonConvert.SerializeObject(command),
                _commandType = command.GetType().AssemblyQualifiedName,
                Schedule = TaskSchedule.Create(frequency, interval)
            };
        }

        /// <summary>
        /// Executes the task using the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>Returns the result of the command.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="container"/> argument is null.</exception>
        public CommandResult Execute(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.LastRun = DateTimeOffset.Now;

            var command = JsonConvert.DeserializeObject(this.Command, Type.GetType(_commandType));

            return container.Resolve<IApplicationBus>().Send((dynamic)command).Result;
        }
    }
}