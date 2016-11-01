using System;

namespace Slalom.Boost
{
    /// <summary>
    /// Specifies the user story and task that a class was developed for.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class UserStoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserStoryAttribute"/> class.
        /// </summary>
        /// <param name="storyId">The story identifier.</param>
        public UserStoryAttribute(string storyId)
            : this(storyId, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStoryAttribute"/> class.
        /// </summary>
        /// <param name="storyId">The story identifier.</param>
        /// <param name="taskId">The task identifier.</param>
        public UserStoryAttribute(string storyId, string taskId)
        {
            this.StoryId = storyId;
            this.TaskId = taskId;
        }

        /// <summary>
        /// Gets the story identifier.
        /// </summary>
        /// <value>The story identifier.</value>
        public string StoryId { get; private set; }

        /// <summary>
        /// Gets the task identifier.
        /// </summary>
        /// <value>The task identifier.</value>
        public string TaskId { get; private set; }
    }
}