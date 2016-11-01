using Slalom.Boost.Commands;

namespace Communication.Client
{
    public class TestCommand : Command<TestEvent>
    {
        public TestCommand(string name)
        {
            this.Name = name;
        }

        public TestCommand()
        {
        }

        public string Name { get; set; }
    }
}