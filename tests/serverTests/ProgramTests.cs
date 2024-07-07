using System.Net.Http.Json;
using Xunit;
using project_frej.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net;
using project_frej.Tests.TestUtils;
using System.Text.Json;

namespace project_frej.Tests.ProgramTests
{
    public class SensorDataApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public SensorDataApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("ApiKey", "testing");
        }
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

        [Fact]
        public async Task Get_SensorDataById_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/sensorData/1");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseData = await response.Content.ReadFromJsonAsync<object>();
            Console.WriteLine(responseData);
            Assert.NotNull(responseData);
        }

        [Fact]
        public async Task Get_SensorDataById_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/sensorData/1000000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_SensorData_ReturnsCreated()
        {
            // Arrange
            var sensorReading = new SensorReading
            {
                Pressure = 200,
                Temperature = 20,
                Humidity = 50,
                Lux = 300,
                Uvs = 1,
                Gas = 10,
                Timestamp = new System.DateTime(1998, 7, 16)
            };

            // add api key as "Authorization" header
            _client.DefaultRequestHeaders.Add("Authorization", "testing");

            // Act
            var response = await _client.PostAsJsonAsync("/api/sensorData", sensorReading);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Get_SensorDataLatest_ReturnsOk()
        {
            // Post a new sensor reading
            var sensorReading = new SensorReading
            {
                Pressure = 2000,
                Temperature = 200,
                Humidity = 500,
                Lux = 3000,
                Uvs = 10,
                Gas = 100,
                Timestamp = new System.DateTime(3000, 1, 1)
            };
            _client.DefaultRequestHeaders.Add("Authorization", "testing");
            await _client.PostAsJsonAsync("/api/sensorData", sensorReading);

            // Act
            var response = await _client.GetAsync("/api/sensorData/latest?pageSize=1");

            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonDocument.Parse(responseString).RootElement;

            // Extract the data part from the response
            var data = responseData.GetProperty("data").EnumerateArray().First();

            // Verify the data
            Assert.Equal(sensorReading.Pressure, data.GetProperty("pressure").GetDouble());
            Assert.Equal(sensorReading.Temperature, data.GetProperty("temperature").GetDouble());
            Assert.Equal(sensorReading.Humidity, data.GetProperty("humidity").GetDouble());
            Assert.Equal(sensorReading.Lux, data.GetProperty("lux").GetDouble());
            Assert.Equal(sensorReading.Uvs, data.GetProperty("uvs").GetDouble());
            Assert.Equal(sensorReading.Gas, data.GetProperty("gas").GetDouble());
        }
    }
}
