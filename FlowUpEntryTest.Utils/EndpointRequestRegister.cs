using System.Collections.Concurrent;
using FlowUpEntryTest.Abstractions;
using FlowUpEntryTest.Data;

namespace Utils
{
    public class EndpointRequestRegister : IEndpointRequestRegister
    {
        private readonly ConcurrentDictionary<string, List<EndpointRequest>> _requests;

        public EndpointRequestRegister()
        {
            _requests = new ConcurrentDictionary<string, List<EndpointRequest>>();
        }

        public void RegisterEndpointRequest(string name, DateTime dateTime)
        {
            var requestsList = _requests.GetOrAdd(name, new List<EndpointRequest>());
  
            requestsList.Add(new EndpointRequest(name, dateTime));
        }

        public int[] GetRequestsCountsWithinInterval(string endpointName,
            DateTime from,
            DateTime to,
            double intervalMinutes)
        {
            var intervalTicks = intervalMinutes * TimeSpan.TicksPerMinute;
            //for manual testing: intervalTicks = intervalMinutes / 12;   //shorten debug interval to 5s instead of 1 minute
            
            if (!_requests.TryGetValue(endpointName, out var requestsList)) return Array.Empty<int>();

            return requestsList
                .Where(request => request.DateTime >= from && request.DateTime <= to)
                .GroupBy(request => request.DateTime.Ticks / intervalTicks)
                .Select(group => group.Count())
                .ToArray();
        }
    }
}