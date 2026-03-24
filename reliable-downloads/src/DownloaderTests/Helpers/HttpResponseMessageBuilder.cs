namespace DownloaderTests.Helpers;

public sealed class HttpResponseMessageBuilder
{
    private readonly HttpResponseMessage _message = new();

    public HttpResponseMessage Build() => _message;

    public HttpResponseMessageBuilder WithAcceptRange()
    {
        _message.Headers.Add("Accept-Ranges", "bytes");
        return this;
    }
    
    public HttpResponseMessageBuilder WithContent(HttpContent content)
    {
        _message.Content = content;
        return this;
    }

    public HttpResponseMessageBuilder WithContentLength(long bytes)
    {
        _message.Content.Headers.ContentLength = bytes;
        return this;
    }
    
    public HttpResponseMessageBuilder WithMd5(byte[] hash)
    {
        _message.Content.Headers.ContentMD5 = hash;
        return this;
    }
}