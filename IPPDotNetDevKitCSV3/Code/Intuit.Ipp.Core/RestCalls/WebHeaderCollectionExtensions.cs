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
            var splitter = i < headers.Count - 1 ? ";" : string.Empty;
            headersStringBuilder.Append(
                $"{headers[i]}: {headers[headers[i]]}{splitter}");
        }

        return headersStringBuilder.ToString();
    }
}
