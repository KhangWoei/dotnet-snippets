using Crawler.LinkVisiting;
using NSubstitute;

namespace Crawler.Tests;

[TestFixture]
public sealed class LinkVisitingTests
{
    [TestCase("some html", true)]
    [TestCase("", false)]
    public Task WhenMediaTypeIsHtml_GetsHtml(string expected, bool isHtml)
    {
        var visitor = CreateLinkVisitor(expected, isHtml);

        var uri = new Uri("https://contoso.com");
        var actual = await visitor.VisitAsync(uri, default);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private ILinkVisitor CreateLinkVisitor(string html, bool isHtml)
    {
        var httpClient = Substitute.For<HttpClient>();
        var response = new HttpResponseMessage();

        httpClient.GetAsync(Arg.Any<Uri>(), Arg.Any<CancellationToken>());

        return new LinkVisitor(httpClient);
    }
}
