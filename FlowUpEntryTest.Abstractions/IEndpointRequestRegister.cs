namespace FlowUpEntryTest.Abstractions
{
    public interface IEndpointRequestRegister
    {
        public void RegisterEndpointRequest(string name, DateTime dateTime);
        
        public int[] GetRequestsCountsWithinInterval(string endpointName,
            DateTime from,
            DateTime to,
            long intervalMinutes);
    }
}