using System.Net;
using FlowUpEntryTest.Data;
using NUnit.Framework;

namespace TestProject1;

/// <summary>
/// Unfinished code of validation test (was part of Bonus section in task specification document).
/// </summary>
public class RestServiceTests
{
    private const string SERVER_URL_BASE = "http://localhost:5237/";
    
    [Test]
    public async Task GetRequestsStatistics_5UsersActing_CorrectStatisticsRetrieved()
    {
        //behavior definitions
        var user1Requests = new List<EndpointRequest>()
        {
            new EndpointRequest("click", DateTime.Now.AddSeconds(1)),
            new EndpointRequest("someOtherClick", DateTime.Now.AddSeconds(5)),
            new EndpointRequest("click", DateTime.Now.AddSeconds(10))
        };
        var user2Requests = new List<EndpointRequest>()
        {
            new EndpointRequest("click", DateTime.Now.AddSeconds(1)),
            new EndpointRequest("someOtherClick", DateTime.Now.AddSeconds(5)),
            new EndpointRequest("click", DateTime.Now.AddSeconds(10))
        };
        var user3Requests = new List<EndpointRequest>()
        {
            new EndpointRequest("click", DateTime.Now.AddSeconds(1)),
            new EndpointRequest("someOtherClick", DateTime.Now.AddSeconds(5)),
            new EndpointRequest("click", DateTime.Now.AddSeconds(10))
        };
        var user4Requests = new List<EndpointRequest>()
        {
            new EndpointRequest("click", DateTime.Now.AddSeconds(1)),
            new EndpointRequest("someOtherClick", DateTime.Now.AddSeconds(5)),
            new EndpointRequest("click", DateTime.Now.AddSeconds(10))
        };
        var user5Requests = new List<EndpointRequest>()
        {
            new EndpointRequest("click", DateTime.Now.AddSeconds(1)),
            new EndpointRequest("someOtherClick", DateTime.Now.AddSeconds(5)),
            new EndpointRequest("click", DateTime.Now.AddSeconds(10))
        };
        var startTime = DateTime.Now;
        
        //act
        var user1Task = SimulateUser(startTime, user1Requests);
        var user2Task = SimulateUser(startTime, user2Requests);
        var user3Task = SimulateUser(startTime, user3Requests);
        var user4Task = SimulateUser(startTime, user4Requests);
        var user5Task = SimulateUser(startTime, user5Requests);
        
        Task.WaitAll(user1Task, user2Task, user3Task, user4Task, user5Task);

        //start verifications
        var httpClient = new HttpClient();
        var startTimestamp = (startTime.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds;
        var endTimestamp = (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
        var response = await httpClient.GetAsync($"{SERVER_URL_BASE}click?from={startTimestamp}&to={endTimestamp}&interval=1");
        
        //TODO finish verification
        //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task SimulateUser(DateTime startTime, List<EndpointRequest> requests)
    {
        var httpClient = new HttpClient();
        var lastRequestFinishedDateTime = (DateTime?)null;
        foreach (var request in requests)
        {
            var requestExecutionDelay = request.DateTime - (lastRequestFinishedDateTime ?? startTime);
            await Task.Delay(requestExecutionDelay);
            await httpClient.PostAsync($"{SERVER_URL_BASE}{request.Name}", null);
            lastRequestFinishedDateTime = DateTime.Now;
        }
    }
}