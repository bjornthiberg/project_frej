using System.Net.Http.Json;
using Xunit;
using project_frej.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net;
using project_frej.Tests.TestUtils;

public class SensorDataApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Get_SensorData_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/sensorData");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseData = await response.Content.ReadFromJsonAsync<object>();
        Assert.NotNull(responseData);
    }
}
