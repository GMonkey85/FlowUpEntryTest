using FlowUpEntryTestWebApp.Abstractions;
using FlowUpEntryTestWebApp.RequestHandlers;
using Utils;

namespace FlowUpEntryTestWebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRequestStatisticsRequestHandler>(
                new RequestStatisticsRequestHandler(
                    new EndpointRequestRegister()
                )
            );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost(
                    "/{relativeUri}",
                    httpContext => GetRequestStatisticsRequestHandlerService(app).PostRequestsToStatistics(httpContext)
                );

                endpoints.MapGet(
                    "/{relativeUri}",
                    httpContext => GetRequestStatisticsRequestHandlerService(app).GetRequestsStatistics(httpContext)
                );
            });
        }

        private static IRequestStatisticsRequestHandler GetRequestStatisticsRequestHandlerService(
            IApplicationBuilder app)
        {
            var requestStatisticsRequestHandler = app.ApplicationServices.GetService<IRequestStatisticsRequestHandler>();

            if (requestStatisticsRequestHandler == null)
            {
                throw new Exception($"{nameof(IRequestStatisticsRequestHandler)} is not correctly initialized.");
            }

            return requestStatisticsRequestHandler;
        }
    }
}