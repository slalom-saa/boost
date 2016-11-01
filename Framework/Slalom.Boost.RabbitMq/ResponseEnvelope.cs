using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Slalom.Boost.RabbitMq
{
    [Serializable]
    public class ResponseEnvelope
    {
        public byte[] Payload { get; set; }

        public Type ResponseType { get; set; }

        public ResponseEnvelope()
        {
        }

        public ResponseEnvelope(object response)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, response);
                this.Payload = stream.ToArray();
            }
            this.ResponseType = response.GetType();
        }

        public object GetResponse()
        {
            using (var stream = new MemoryStream(this.Payload))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
    }
}