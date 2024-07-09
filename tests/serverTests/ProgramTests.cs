using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using project_frej.Models;
using project_frej.Tests.TestUtils;

namespace project_frej.Tests.ProgramTests
{
    public class SensorDataApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Get_SensorData_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/sensorData");
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<object>();

            Assert.NotNull(responseData);
        }

        [Fact]
        public async Task Get_SensorDataById_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/sensorData/1");

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<object>();

            Assert.NotNull(responseData);
        }

        [Fact]
        public async Task Get_SensorDataById_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/sensorData/1000000");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidSensorData_ReturnsOk()
        {
            var sensorReading = new SensorReading
            {
                Pressure = 200,
                Temperature = 20,
                Humidity = 50,
                Lux = 300,
                Uvs = 1,
                Gas = 10,
                Timestamp = new DateTime(1998, 7, 16)
            };

            _client.DefaultRequestHeaders.Add("Authorization", "testing");

            var response = await _client.PostAsJsonAsync("/api/sensorData", sensorReading);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Post_InvalidSensorData_ReturnsInternalServerError()
        {
            var invalidSensorReadingJson = @"
            {
                ""Pressure"": ""shouldnotbeastring"",
                ""Temperature"": 20,
                ""Humidity"": 50,
                ""Lux"": 300,
                ""Uvs"": 1,
                ""Gas"": 10,
                ""Timestamp"": ""1998-07-16T00:00:00""
            }";

            var content = new StringContent(invalidSensorReadingJson, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("Authorization", "testing");

            var response = await _client.PostAsync("/api/sensorData", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_SensorDataLatest_ReturnsOk()
        {
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

            var response = await _client.GetAsync("/api/sensorData/latest?pageSize=1");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonDocument.Parse(responseString).RootElement;

            var data = responseData.GetProperty("data").EnumerateArray().First();

            Assert.Equal(sensorReading.Pressure, data.GetProperty("pressure").GetDouble());
            Assert.Equal(sensorReading.Temperature, data.GetProperty("temperature").GetDouble());
            Assert.Equal(sensorReading.Humidity, data.GetProperty("humidity").GetDouble());
            Assert.Equal(sensorReading.Lux, data.GetProperty("lux").GetDouble());
            Assert.Equal(sensorReading.Uvs, data.GetProperty("uvs").GetDouble());
            Assert.Equal(sensorReading.Gas, data.GetProperty("gas").GetDouble());
        }
    }
}
