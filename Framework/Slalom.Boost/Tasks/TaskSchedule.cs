namespace Slalom.Boost.Tasks
{
    /// <summary>
    /// Represents a schedule for a task.
    /// </summary>
    public class TaskSchedule
    {
        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public int Interval { get; private set; }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public TaskFrequency Frequency { get; private set; }

        /// <summary>
        /// Gets the manual task schedule.
        /// </summary>
        /// <value>The manual task schedule.</value>
        public static TaskSchedule Manual => Create(TaskFrequency.Manual, 0);

        /// <summary>
        /// Creates a new task schedule.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>Retursn the created task schedule.</returns>
        public static TaskSchedule Create(TaskFrequency frequency, int interval)
        {
            if (frequency != TaskFrequency.Manual)
            {
                Argument.GreaterThan(() => interval, 0);
            }

            return new TaskSchedule
            {
                Frequency = frequency,
                Interval = interval
            };
        }
    }
}