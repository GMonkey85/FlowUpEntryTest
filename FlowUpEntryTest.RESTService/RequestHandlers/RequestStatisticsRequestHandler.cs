using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using FlowUpEntryTest.Abstractions;
using FlowUpEntryTestWebApp.Abstractions;

namespace FlowUpEntryTestWebApp.RequestHandlers;

public class RequestStatisticsRequestHandler : IRequestStatisticsRequestHandler
{
    private readonly IEndpointRequestRegister _requestRegisterService;
    
    const string MESSAGE_INTERNAL_SERVER_ERROR = "Internal server error.";

    public RequestStatisticsRequestHandler(IEndpointRequestRegister requestRegisterService)
    {
        if (requestRegisterService == null) throw new ArgumentNullException(nameof(requestRegisterService));

        _requestRegisterService = requestRegisterService;
    }

    public Task PostRequestsToStatistics(HttpContext httpContext)
    {
        try
        {
            var requestUtcDateTime = DateTime.UtcNow;

            var endpointName = ParseRequiredEndpointName(httpContext);

            _requestRegisterService.RegisterEndpointRequest(endpointName, requestUtcDateTime);

            return httpContext.Response.WriteAsync($"Registered call of {endpointName} at {requestUtcDateTime}");
        }
        catch (BadHttpRequestException e)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return httpContext.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            Console.WriteLine(e.Message);
            return httpContext.Response.WriteAsync(MESSAGE_INTERNAL_SERVER_ERROR);
        }
    }

    public Task GetRequestsStatistics(HttpContext httpContext)
    {
        try
        {
            var endpointName = ParseRequiredEndpointName(httpContext);

            var query = httpContext.Request.Query;

            var fromDateTime = ParseTimestampParameter("from", query);
            var toDateTime = ParseTimestampParameter("to", query);
            var intervalMinutes = ParseTimestampIntervalParameter("interval", query);

            var responseData = _requestRegisterService.GetRequestsCountsWithinInterval(
                endpointName,
                fromDateTime,
                toDateTime,
                intervalMinutes
            );

            var jsonRoot = new JsonArray();
            foreach (var responseDataItem in responseData)
            {
                jsonRoot.Add(new JsonObject {[endpointName] = responseDataItem});
            }

            return httpContext.Response.WriteAsJsonAsync(jsonRoot, new JsonSerializerOptions() {WriteIndented = true});
        }
        catch (BadHttpRequestException e)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return httpContext.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            Console.WriteLine(e.Message);
            return httpContext.Response.WriteAsync(MESSAGE_INTERNAL_SERVER_ERROR);
        }
    }
    
    private static string ParseRequiredEndpointName(HttpContext httpContext)
    {
        var endpointName = httpContext.Request.RouteValues["relativeUri"]?.ToString();
        if (endpointName == null)
        {
            throw new BadHttpRequestException("Uri must contain valid endpoint name specification.");
        }

        return endpointName;
    }

    private static DateTime ParseTimestampParameter(string parameterName, IQueryCollection query)
    {
        if (!long.TryParse(query[parameterName], out var unixTimestamp)
            || unixTimestamp < 0)
        {
            throw new BadHttpRequestException($"Parameter '{parameterName}' must be valid UNIX timestamp.");
        }

        return DateTime.UnixEpoch.AddSeconds(unixTimestamp);
    }

    private static long ParseTimestampIntervalParameter(string parameterName, IQueryCollection query)
    {
        if (!long.TryParse(query["interval"], out var intervalMinutes)
            || intervalMinutes < 1)
        {
            throw new BadHttpRequestException($"Parameter '{parameterName}' must decimal number (long) > 1.");
        }

        return intervalMinutes;
    }
}