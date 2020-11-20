using ArraySorter.Api.Controllers.Job;
using System;
using Xunit;

namespace ArraySorter.Api.UnitTest
{
    public abstract class JobControllerTest
    {
        public sealed class When_Creating_Instance : JobControllerTest
        {
            [Fact]
            public void It_throws_when_logger_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(default));
            }
        }
    }
}
