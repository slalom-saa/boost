using System;
using Communication.Client;
using Slalom.Boost.Commands;

namespace Communication.Service1
{
    public class TestCommandHandler : CommandHandler<TestCommand, TestEvent>
    {
        public override TestEvent HandleCommand(TestCommand command)
        {
            Console.WriteLine("Command handle from Service 1 for {0}.", command.Name);

            return new TestEvent(command.Name);
        }
    }
}