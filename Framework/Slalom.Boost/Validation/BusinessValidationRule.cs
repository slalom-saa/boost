using System;
using System.Collections.Generic;
using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Provides a base class for business validation rules.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="Slalom.Boost.Validation.IBusinessValidationRule{TCommand}" />
    public abstract class BusinessValidationRule<TCommand> : IHaveCommandContext,
                                                             IHaveDataFacade,
                                                             IBusinessValidationRule<TCommand> where TCommand : ICommand

    {
        /// <summary>
        /// Gets the order of the business rule.  Lower numbers run fist.
        /// </summary>
        /// <value>
        /// The order of the business rule.
        /// </value>
        public int Order { get; protected set; }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="context">The current context that can be used to share information
        /// between validation rules.</param>
        /// <returns>Returns all found validation errors.</returns>
        public IEnumerable<ValidationMessage> Validate(TCommand instance, CommandContext context)
        {
            this.Context = context;

            return this.Validate(instance);
        }

        /// <summary>f
        /// Gets or sets the current command context.
        /// </summary>
        /// <value>The current command context.</value>
        public CommandContext Context { get; private set; }

        /// <summary>
        /// Sets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CommandContext"/>.</param>
        public void SetContext(CommandContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the current <see cref="IDataFacade"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IDataFacade"/> instance.
        /// </value>
        [RuntimeBindingDependency]
        public IDataFacade DataFacade { get; set; }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>Returns all found validation errors.</returns>
        public abstract IEnumerable<ValidationMessage> Validate(TCommand instance);
    }
}