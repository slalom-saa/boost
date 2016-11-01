using System;
using Communication.Client;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Communication.Service2
{
    public class TestEventHandler : IHandleEvent<TestEvent>
    {
        public void Handle(TestEvent instance, CommandContext context)
        {
            Console.WriteLine("Event handle from Service 2 for {0}.", instance.Name);
        }
    }
}