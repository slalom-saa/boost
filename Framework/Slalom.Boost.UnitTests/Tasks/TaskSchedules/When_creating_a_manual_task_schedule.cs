using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using Slalom.Boost.Tasks;

namespace Slalom.Boost.UnitTests.Tasks.TaskSchedules
{
    public class When_creating_a_manual_task_schedule
    {
        Because of = () => Exception = Catch.Exception(() => TaskSchedule.Manual);

        static Exception Exception;

        private It should_not_throw_an_exception = () => Exception.ShouldBeNull();
    }
}
