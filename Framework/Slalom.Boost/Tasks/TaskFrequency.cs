namespace Slalom.Boost.Tasks
{
    /// <summary>
    /// Indicates the task frequency.
    /// </summary>
    public enum TaskFrequency
    {
        /// <summary>
        /// Indicates the task should be run manually.
        /// </summary>
        Manual,

        /// <summary>
        /// Indicates the task should run based on seconds.
        /// </summary>
        Seconds,

        /// <summary>
        /// Indicates the task should run based on minutes.
        /// </summary>
        Minute,

        /// <summary>
        /// Indicates the task should run based on hours.
        /// </summary>
        Hour,

        /// <summary>
        /// Indicates the task should run based on days.
        /// </summary>
        Day
    }
}