using System;
using Slalom.Boost.Events;

namespace Communication.Client
{
    [Serializable]
    public class TestEvent : Event
    {
        public TestEvent(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}