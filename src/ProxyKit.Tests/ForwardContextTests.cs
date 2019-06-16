using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace ProxyKit
{
    public class ForwardContextTests
    {
        private readonly DefaultHttpContext _httpContext;

        public ForwardContextTests()
        {
            _httpContext = new DefaultHttpContext();
            var services = new ServiceCollection();
            services.AddHttpClient<ProxyKitClient>();
            var serviceProvider = services.BuildServiceProvider();
            _httpContext.RequestServices = serviceProvider;
        }

        [Theory]
        [InlineData("/", "http://localhost/a", "", "/a")]
        [InlineData("/a", "http://localhost/a", "", "/a")]
        [InlineData("/a", "http://localhost/a", "/", "/a/")]
        [InlineData("/b", "http://localhost/b/", "", "/b/")]
        [InlineData("/b", "http://localhost/b/", "/", "/b//")]
        public void Should_create_correct_upstream_request_uri(string pathBase, string forwardTo, string request, string expectedPath)
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = request;
            _httpContext.Request.PathBase = pathBase;

            var forwardContext = _httpContext.ForwardTo(forwardTo);

            forwardContext.UpstreamRequest.RequestUri.AbsolutePath.ShouldBe(expectedPath);
        }
    }
}
