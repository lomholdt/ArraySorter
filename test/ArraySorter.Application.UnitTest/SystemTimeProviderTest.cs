using Xunit;

namespace ArraySorter.Application.UnitTest
{
    public abstract class SystemTimeProviderTest
    {
        public class When_Getting_UtcNow
        {
            private readonly SystemTimeProvider _sut;

            public When_Getting_UtcNow()
            {
                _sut = new SystemTimeProvider();
            }

            [Fact]
            public void It_provides_the_time()
            {
                Assert.NotEqual(default, _sut.UtcNow);
            }
        }
    }
}
