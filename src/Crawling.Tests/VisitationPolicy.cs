using System.Net;
using System.Net.Http.Headers;
using Crawling.Frontier;
using Crawling.LinkVisiting;
using NSubstitute;

namespace Crawler.Tests.LinkVisiting;

[TestFixture]
public sealed class VisitationPolicy
{
    [Test]
    public async Task True_WhenMediaTypeIsHtml()
    {
        var policy = CreateVisitationPolicy(true);
        var uri = new Uri("https://contoso.com");

        var actual = await policy.ShouldVisit(uri, CancellationToken.None);

        Assert.That(actual, Is.True);
    }
    
    [Test]
    public async Task False_WhenMediaTypeIsNotHtml()
    {
        var policy = CreateVisitationPolicy(false);
        var uri = new Uri("https://contoso.com");

        var actual = await policy.ShouldVisit(uri, CancellationToken.None);

        Assert.That(actual, Is.False);
    }

    private static IVisitationPolicy CreateVisitationPolicy(bool isHtml)
    {
        var factory = Substitute.For<IHttpClientFactory>();
        var client = CreateHttpClient("", isHtml);
        factory.CreateClient(Arg.Any<string>()).Returns(client);

        return new Crawling.Frontier.VisitationPolicy(factory);
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
