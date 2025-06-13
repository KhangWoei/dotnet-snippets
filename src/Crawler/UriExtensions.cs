using System.Diagnostics.CodeAnalysis;

namespace Crawler;

public static class UriExtensions
{
    public static bool TryCreateRelativeOrAbsolute(this Uri baseUri, string relativeOrAbsolute, [MaybeNullWhen(false)] out Uri output)
    {
        output = null;

        if (Uri.IsWellFormedUriString(relativeOrAbsolute, UriKind.Absolute))
        {
            output = new Uri(relativeOrAbsolute, UriKind.Absolute);
            return true;
        }

        if (Uri.IsWellFormedUriString(relativeOrAbsolute, UriKind.Relative))
        {
            var relative = new Uri(relativeOrAbsolute, UriKind.Relative);
            output = new Uri(baseUri, relative);
            return true;
        }

        return false;
    }
}
