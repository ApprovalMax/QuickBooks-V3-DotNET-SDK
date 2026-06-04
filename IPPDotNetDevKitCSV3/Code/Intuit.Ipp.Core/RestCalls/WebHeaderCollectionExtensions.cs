using System.Linq;
using System.Net;
using System.Text;

namespace Intuit.Ipp.Core.RestCalls;

/// <summary>
/// Extension methods for <see cref="WebHeaderCollection"/>
/// </summary>
public static class WebHeaderCollectionExtensions
{
    /// <summary>
    /// Convert <see cref="WebHeaderCollection"/> to string 
    /// </summary>
    /// <param name="headers">The <see cref="WebHeaderCollection"/></param>
    /// <returns>The headers string</returns>
    public static string ConvertHeaderToString(this WebHeaderCollection headers)
    {
        var headersStringBuilder = new StringBuilder();
        for (var i = 0; i < headers.Count; i++)
        {
            var splitter = i < headers.Count - 1
                ? ";"
                : string.Empty;

            // headers.GetKey(i) is the header name; headers.Get(i) is its value.
            // The indexer headers[i] returns the value, not the name, so it must not be used as the key.
            headersStringBuilder.Append($"{headers.GetKey(i)}: {headers.Get(i)}{splitter}");
        }

        return headersStringBuilder.ToString();
    }
}
