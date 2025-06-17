using Crawler.LinkVisiting;
using System.Net;
using System.Net.Http.Headers;

namespace Crawler.Tests;

[TestFixture]
public sealed class LinkVisitingTests
{
    [TestCase("some html", true)]
    [TestCase("", false)]
    public async Task WhenMediaTypeIsHtml_GetsHtml(string expected, bool isHtml)
    {
        var visitor = CreateLinkVisitor(expected, isHtml);

        var uri = new Uri("https://contoso.com");
        var actual = await visitor.VisitAsync(uri, default);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private ILinkVisitor CreateLinkVisitor(string html, bool isHtml)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(html),
        };

        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

        return new LinkVisitor(new HttpClient(new FakeMessageHandler(response)));
    }

    private class FakeMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}
