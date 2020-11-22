using Microsoft.AspNetCore.Http;
using NSubstitute;
using ArraySorter.Api.Middleware;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ArraySorter.Domain.Exceptions;

namespace ArraySorter.Api.UnitTest.Middleware
{
    public abstract class ExceptionHandlingMiddlewareTest
    {
        private static bool _nextInvoked = false;

        private readonly ExceptionHandlingMiddleware _sut;

        private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

        private readonly RequestDelegate _next = _ =>
        {
            _nextInvoked = true;

            return Task.CompletedTask;
        };

        private readonly Stream _responseStream = new MemoryStream();

        private static RequestDelegate ThrowableRequestDelegate(Exception e) => _ => throw e;

        public ExceptionHandlingMiddlewareTest()
        {
            _httpContext.Response.Body = _responseStream;

            _sut = new ExceptionHandlingMiddleware(_next);
        }

        public sealed class When_Creating_Instance
        {
            [Fact]
            public void It_throws_if_next_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new ExceptionHandlingMiddleware(default));
            }
        }

        public sealed class When_Hitting_Middleware : ExceptionHandlingMiddlewareTest
        {
            [Fact]
            public async Task It_invokes_next()
            {
                await _sut.Invoke(_httpContext);

                Assert.True(_nextInvoked);
            }

            [Fact]
            public async Task It_returns_bad_request_when_invalid_status_is_thrown()
            {
                const int expected = StatusCodes.Status400BadRequest;

                var e = new InvalidStatusException();

                var next = ThrowableRequestDelegate(e);

                var sut = new ExceptionHandlingMiddleware(next);

                await sut.Invoke(_httpContext);

                var actual = _httpContext.Response.StatusCode;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public async Task It_returns_bad_request_when_empty_input_is_thrown()
            {
                const int expected = StatusCodes.Status400BadRequest;

                var e = new EmptyInputException();

                var next = ThrowableRequestDelegate(e);

                var sut = new ExceptionHandlingMiddleware(next);

                await sut.Invoke(_httpContext);

                var actual = _httpContext.Response.StatusCode;

                Assert.Equal(expected, actual);
            }
        }
    }
}
