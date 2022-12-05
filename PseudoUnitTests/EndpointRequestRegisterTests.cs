using System;
using NUnit.Framework;
using Utils;

namespace UnitTests;

[TestFixture]
public class EndpointRequestRegisterTests
{
    [Test]
    public void GetRequestsCountsWithinInterval_RequestsCountMatches()
    {
        var endpointRequestRegister = new EndpointRequestRegister();
        
        //click before interval
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2000, 1, 1, 1, 1, 1));
        
        //click within interval
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2022, 1, 1, 1, 1, 1));
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2022, 1, 1, 1, 1, 2));
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2022, 1, 1, 1, 2, 1));
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2022, 1, 1, 1, 2, 2));
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2022, 1, 1, 1, 2, 3));
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2022, 1, 1, 1, 3, 1));
        
        //click after interval
        endpointRequestRegister.RegisterEndpointRequest("click", new DateTime(2023, 1, 1, 1, 1, 1));
        
        //otherAction within interval
        endpointRequestRegister.RegisterEndpointRequest("otherAction", new DateTime(2022, 1, 1, 1, 1, 1));

        var clickCounts = endpointRequestRegister.GetRequestsCountsWithinInterval(
            "click",
            new DateTime(2022, 1, 1, 1, 1, 1),
            new DateTime(2022, 1, 1, 1, 3, 1),
            1
        );

        Assert.AreEqual(2, clickCounts[0]);
        Assert.AreEqual(3, clickCounts[1]);
        Assert.AreEqual(1, clickCounts[2]);
    }
}