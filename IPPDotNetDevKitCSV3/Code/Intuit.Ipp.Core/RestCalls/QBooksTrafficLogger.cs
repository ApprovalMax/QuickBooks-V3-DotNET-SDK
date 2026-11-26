using System.Collections.Generic;
using System.Net;
using Intuit.Ipp.Core.RestCalls;
using Intuit.Ipp.Diagnostics;

namespace Intuit.Ipp.Core.Rest;

/// <summary>
/// Logs QBooks HTTP traffic through the <see cref="Configuration.Logger.CustomLogger"/>. The one place that
/// decides how a request, its response or its failure is logged: the message, the level and what
/// <see cref="Configuration.Logger.UseVerboseLogging"/> adds. Callers only report what happened.
/// </summary>
/// <remarks>
/// Without verbose logging only failures are logged, one line each, without the response body.
/// Verbose logging adds the requests, the successful responses and the response bodies.
/// </remarks>
internal sealed class QBooksTrafficLogger
{
    private const string RequestTemplate =
        "QBooks request {Method} {RequestUri}. RealmId: {RealmId}; Body: {Content}.";

    private const string RequestWithoutBodyTemplate =
        "QBooks request {Method} {RequestUri}. RealmId: {RealmId}.";

    private const string SucceededTemplate =
        "QBooks request {Method} {RequestUri} succeeded with status code {StatusCode}. RealmId: {RealmId} Intuit_Tid: {IntuitTid}; Headers: {Headers}; Body: {Content}.";

    private const string FailedWithStatusTemplate =
        "QBooks request {Method} {RequestUri} failed with status code {StatusCode}. RealmId: {RealmId} Intuit_Tid: {IntuitTid}; Exception: {Exception}; Headers: {Headers}";

    private const string FaultInResponseTemplate =
        "QBooks request {Method} {RequestUri} returned a fault with status code {StatusCode}. RealmId: {RealmId} Intuit_Tid: {IntuitTid}; Exception: {Exception}; Headers: {Headers}";

    private const string BodySuffix = "; Body: {Content}.";

    private const string FailedWithoutResponseTemplate =
        "QBooks request {Method} {RequestUri} failed without a response. RealmId: {RealmId} Status: {WebExceptionStatus}; Exception: {Exception}.";

    private const string FailedTemplate =
        "QBooks request {Method} {RequestUri} failed. RealmId: {RealmId}; Exception: {Exception}.";

    private readonly ServiceContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="QBooksTrafficLogger"/> class.
    /// </summary>
    /// <param name="context">The service context, holding the logger and the realm.</param>
    public QBooksTrafficLogger(ServiceContext context)
    {
        this.context = context;
    }

    private Configuration.Logger Logger => this.context.IppConfiguration.Logger;

    /// <summary>
    /// Logs a request about to be sent, when verbose logging is enabled.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="body">The request body, empty for a request without one.</param>
    public void LogRequest(HttpWebRequest request, string body)
    {
        if (!this.Logger.UseVerboseLogging)
        {
            return;
        }

        if (string.IsNullOrEmpty(body))
        {
            this.Logger.CustomLogger.Log(
                TraceLevel.Info,
                RequestWithoutBodyTemplate,
                request?.Method,
                request?.RequestUri,
                this.context.RealmId);

            return;
        }

        this.Logger.CustomLogger.Log(
            TraceLevel.Info,
            RequestTemplate,
            request?.Method,
            request?.RequestUri,
            this.context.RealmId,
            body);
    }

    /// <summary>
    /// Logs a successful response, when verbose logging is enabled.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="response">The response.</param>
    public void LogResponse(HttpWebRequest request, QBooksResponse response)
    {
        if (!this.Logger.UseVerboseLogging)
        {
            return;
        }

        this.Logger.CustomLogger.Log(
            TraceLevel.Info,
            SucceededTemplate,
            request?.Method,
            request?.RequestUri,
            response.StatusCode,
            this.context.RealmId,
            response.IntuitTid,
            response.Headers?.ConvertHeaderToString(),
            response.Body);
    }

    /// <summary>
    /// Logs a failed request as a single line, whether verbose logging is enabled or not. Callers must not
    /// log the exception again.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="response">
    /// The response QBooks returned: an error status, or a fault in a successful response. Null when the
    /// request failed without one, e.g. on a timeout, a dropped connection or after all retries.
    /// </param>
    /// <param name="exception">The exception the request failed with.</param>
    public void LogFailure(HttpWebRequest request, QBooksResponse response, System.Exception exception)
    {
        if (response == null)
        {
            this.LogFailureWithoutResponse(request, exception);

            return;
        }

        string template = IsSuccessStatusCode(response.StatusCode) ? FaultInResponseTemplate : FailedWithStatusTemplate;
        var args = new List<object>
        {
            request?.Method,
            request?.RequestUri,
            response.StatusCode,
            this.context.RealmId,
            response.IntuitTid,
            exception?.ToString(),
            response.Headers?.ConvertHeaderToString(),
        };

        if (this.Logger.UseVerboseLogging)
        {
            template += BodySuffix;
            args.Add(response.Body);
        }
        else
        {
            template += ".";
        }

        this.Logger.CustomLogger.Log(TraceLevel.Error, template, args.ToArray());
    }

    private static bool IsSuccessStatusCode(int statusCode) => statusCode >= 200 && statusCode <= 299;

    private void LogFailureWithoutResponse(HttpWebRequest request, System.Exception exception)
    {
        if (exception is WebException webException)
        {
            this.Logger.CustomLogger.Log(
                TraceLevel.Error,
                FailedWithoutResponseTemplate,
                request?.Method,
                request?.RequestUri,
                this.context.RealmId,
                webException.Status,
                webException.ToString());

            return;
        }

        this.Logger.CustomLogger.Log(
            TraceLevel.Error,
            FailedTemplate,
            request?.Method,
            request?.RequestUri,
            this.context.RealmId,
            exception?.ToString());
    }
}
