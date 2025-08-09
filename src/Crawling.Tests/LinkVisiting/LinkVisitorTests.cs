using System.Net;
using Crawling;
using Crawling.LinkVisiting;

namespace Crawler.Tests.LinkVisiting;

[TestFixture]
public class LinkVisitorTests
{
    [Test]
    public async Task WhenVisiting_GetsContent()
    {
        var expected = "some content";
        var linkVisitor = new LinkVisitor(new FakeHttpClientFactory(expected), new FakeLinkVisitor(true));


        var actual = await linkVisitor.VisitAsync(new Uri("https://contoso.com"), CancellationToken.None);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private class FakeHttpClientFactory(string expected) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient(new FakeMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expected)
            }));
        }
    }

    private class FakeLinkVisitor(bool shouldVisit) : IVisitationPolicy
    {
        public Task<bool> ShouldVisit(Uri uri, CancellationToken cancellationToken)
        {
            return Task.FromResult(shouldVisit);
        }

        public Task<bool> ShouldVisit(string seed, CancellationToken cancellationToken)
        {
            return Task.FromResult(shouldVisit);
        }
    }

    private class FakeMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}