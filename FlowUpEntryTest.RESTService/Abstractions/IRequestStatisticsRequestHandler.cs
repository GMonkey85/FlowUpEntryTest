namespace FlowUpEntryTestWebApp.Abstractions;

public interface IRequestStatisticsRequestHandler
{
    Task PostRequestsToStatistics(HttpContext httpContext);
    Task GetRequestsStatistics(HttpContext httpContext);
}