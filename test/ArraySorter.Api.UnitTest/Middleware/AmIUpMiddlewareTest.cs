using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ArraySorter.Api.Middleware;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;
using System.Linq;

namespace ArraySorter.Api.UnitTest.Middleware
{
    public abstract class AmIUpMiddlewareTest
    {
        private static bool _nextInvoked = false;

        private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

        private readonly RequestDelegate _next = _ =>
        {
            _nextInvoked = true;

            return Task.CompletedTask;
        };

        private readonly Stream _responseStream = new MemoryStream();

        public sealed class When_Creating_Instance
        {
            [Fact]
            public void It_throws_if_next_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new AmIUpMiddleware(default));
            }
        }

        public sealed class When_Hitting_Middleware : AmIUpMiddlewareTest
        {
            private readonly AmIUpMiddleware _sut;

            public When_Hitting_Middleware()
            {
                _httpContext.Response.Body.Returns(_responseStream);

                _sut = new AmIUpMiddleware(_next);
            }

            [Fact]
            public async Task It_invokes_next()
            {
                await _sut.Invoke(_httpContext);

                Assert.True(_nextInvoked);
            }

            [Fact]
            public async Task It_does_not_invoke_next_when_path_is_root()
            {
                _httpContext.Request.Path.Returns(new PathString("/"));

                await _sut.Invoke(_httpContext);

                Assert.False(_nextInvoked);
            }

            [Fact]
            public async Task It_returns_ok_when_path_is_root()
            {
                _httpContext.Request.Path.Returns(new PathString("/"));

                var expected = "ok";

                await _sut.Invoke(_httpContext);

                _responseStream.Position = 0;

                using var streamReader = new StreamReader(_responseStream);

                var actual = await streamReader.ReadLineAsync();

                Assert.Equal(expected, actual);
            }
        }
    }
}
