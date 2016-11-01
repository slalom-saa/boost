using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Slalom.Boost.Commands;

namespace Slalom.Boost.RabbitMq
{
    [Serializable]
    public class CommandEnvelop
    {
        public string CommandPayload { get; set; }

        public Type CommandType { get; set; }

        public CommandEnvelop()
        {
        }

        public CommandEnvelop(ICommand command, CommandContext context)
        {
            this.CommandPayload = JsonConvert.SerializeObject(command);
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, context);
                this.ContextPayload = stream.ToArray();
            }
            this.CommandType = command.GetType();
        }

        public byte[] ContextPayload { get; set; }

        public ICommand GetCommand()
        {
            return (ICommand)JsonConvert.DeserializeObject(this.CommandPayload, this.CommandType);
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