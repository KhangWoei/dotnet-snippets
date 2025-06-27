using System.Net;
using System.Net.Http.Headers;
using Crawling.LinkVisiting;

namespace Crawler.Tests.LinkVisiting;

[TestFixture]
public sealed class LinkVisitingTests
{
    [Test]
    public async Task WhenMediaTypeIsHtml_GetsHtml()
    {
        
        var visitor = new LinkVisitor();
        var expected = "some html";
        
        var uri = new Uri("https://contoso.com");
        var client = CreateHttpClient(expected, true);
        var actual = await visitor.VisitAsync(client, uri, default);

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task WhenMediaTypeIsNotHtml_ReturnsNull()
    {
        
        var visitor = new LinkVisitor();
        
        var uri = new Uri("https://contoso.com");
        var client = CreateHttpClient("some html", false);
        var actual = await visitor.VisitAsync(client, uri, default);

        Assert.That(actual, Is.Null);
    }

    private static HttpClient CreateHttpClient(string html, bool isHtml)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(html)
        };

        response.Content.Headers.ContentType = isHtml
            ? new MediaTypeHeaderValue("text/html")
            : new MediaTypeHeaderValue("text/plain");

        return new HttpClient(new FakeMessageHandler(response));
    }

    private class FakeMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}
