using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.Domain.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Commands.Sagas
{
    [GeneratedCode("TODO", "1")]
    public abstract class Saga<TSaga> : ISaga where TSaga : Saga<TSaga>
    {
        public DateTime Started { get; private set; } = DateTime.Now;

        [RuntimeBindingDependency]
        protected ISagaStore SagaStore { get; set; }

        [RuntimeBindingDependency]
        protected IContainer Container { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public CommandContext Context { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="IDataFacade"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IDataFacade"/> instance.
        /// </value>
        [RuntimeBindingDependency]
        private IMessageBus Bus { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="IDataFacade"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IDataFacade"/> instance.
        /// </value>
        [RuntimeBindingDependency]
        protected IDataFacade Data { get; set; }

        public Guid Id { get; private set; } = Guid.NewGuid();

        public void RaiseEvents(Event @event)
        {
            this.RaiseEvents(new[] { @event });
        }

        public void RaiseEvents(IEnumerable<Event> events)
        {
            this.RaiseEvents(events.ToArray());
        }

        public void RaiseEvents(Event[] events)
        {
            this.Bus.Send(events);
        }

        public void Save()
        {
            this.SagaStore.Save((TSaga)this);
        }

        public bool Exists(Expression<Func<TSaga, bool>> predicate)
        {
            return SagaStore.Exists(predicate);
        }

        protected ISaga Find(Guid sagaId)
        {
            return this.SagaStore.Find<TSaga>(sagaId);
        }

        protected TSaga Start()
        {
            return (TSaga)this.Container.Resolve(this.GetType());
        }
    }
}