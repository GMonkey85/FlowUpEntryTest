namespace FlowUpEntryTest.Data
{
    public class EndpointRequest
    {
        public string Name { get; }
        public DateTime DateTime { get; }

        public EndpointRequest(string name, DateTime dateTime)
        {
            Name = name;
            DateTime = dateTime;
        }
    }
}