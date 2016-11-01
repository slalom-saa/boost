using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Slalom.Boost.RabbitMq
{
    [Serializable]
    public class EventEnvelope
    {
        public byte[] EventPayload { get; set; }

        public Type EventType { get; set; }

        public EventEnvelope()
        {
        }

        public EventEnvelope(IEvent instance, CommandContext context)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, instance);
                this.EventPayload = stream.ToArray();
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, context);
                this.ContextPayload = stream.ToArray();
            }
            this.EventType = instance.GetType();
        }

        public byte[] ContextPayload { get; set; }

        public IEvent GetEvent()
        {
            using (var stream = new MemoryStream(this.EventPayload))
            {
                var formatter = new BinaryFormatter();
                return (IEvent)formatter.Deserialize(stream);
            }
        }

        public CommandContext GetContext()
        {
            using (var stream = new MemoryStream(this.ContextPayload))
            {
                var formatter = new BinaryFormatter();
                return (CommandContext)formatter.Deserialize(stream);
            }
        }
    }
}