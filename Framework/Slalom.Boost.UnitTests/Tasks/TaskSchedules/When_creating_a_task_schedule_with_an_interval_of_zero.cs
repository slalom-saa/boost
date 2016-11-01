using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using Slalom.Boost.Tasks;

namespace Slalom.Boost.UnitTests.Tasks.TaskSchedules
{
    public class When_creating_a_task_schedule_with_an_interval_of_zero
    {
        Because of = () => Exception = Catch.Exception(() => TaskSchedule.Create(TaskFrequency.Minute, 0));

        static Exception Exception;

        private It should_throw_an_exception = () => Exception.ShouldNotBeNull();

        It should_throw_an_out_of_range_exception = () => Exception.ShouldBeAssignableTo<ArgumentOutOfRangeException>();
    }
}
