using System.Net;

namespace Intuit.Ipp.Core.Rest;

/// <summary>
/// What QBooks returned for a request, captured from the HTTP response so that the
/// <see cref="QBooksTrafficLogger"/> can log it with the outcome of the request.
/// </summary>
internal sealed class QBooksResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QBooksResponse"/> class.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="body">The response body, already read from the response stream.</param>
    public QBooksResponse(HttpWebResponse response, string body)
    {
        this.StatusCode = (int)response.StatusCode;
        this.Headers = response.Headers;
        this.IntuitTid = response.Headers[FaultHandler.IntuitTidExceptionDataKey];
        this.Body = body;
    }

    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Gets the response headers.
    /// </summary>
    public WebHeaderCollection Headers { get; }

    /// <summary>
    /// Gets the intuit_tid response header, or null when QBooks did not send one.
    /// </summary>
    public string IntuitTid { get; }

    /// <summary>
    /// Gets the response body.
    /// </summary>
    public string Body { get; }
}
