using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using project_frej.Middleware;

namespace project_frej.Tests.Middleware
{
    public class ApiKeyPostMiddlewareTests
    {
        private static IHostBuilder CreateHostBuilder(string endpoint, Action<IEndpointRouteBuilder> configure)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            { "ApiKey", "supersecretkey" }
                        });
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddLogging(builder =>
                        {
                            builder.ClearProviders(); // remove all logging providers
                            builder.AddFilter("Microsoft", LogLevel.None); // disable Microsoft logs
                        });
                    })
                    .Configure(app =>
                    {
                        app.UseMiddleware<ApiKeyPostMiddleware>();
                        app.UseRouting();
                        app.UseEndpoints(configure);
                    });
                });
        }

        [Fact]
        public async Task ContinuesForNonPostRequest()
        {
            var host = await CreateHostBuilder("/testGet", endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("GET request received");
                });
            }).StartAsync();

            var client = host.GetTestClient();
            var response = await client.GetAsync("/");

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal("GET request received", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ReturnsUnauthorizedForMissingApiKey()
        {
            var host = await CreateHostBuilder("/", endpoints =>
            {
                endpoints.MapPost("/", async context =>
                {
                    await context.Response.WriteAsync("Received");
                });
            }).StartAsync();

            var client = host.GetTestClient();
            var response = await client.PostAsync("/", new StringContent(""));

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task ReturnsUnauthorizedForInvalidApiKey()
        {
            var host = await CreateHostBuilder("/", endpoints =>
            {
                endpoints.MapPost("/", async context =>
                {
                    await context.Response.WriteAsync("Received");
                });
            }).StartAsync();

            var client = host.GetTestClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/");
            request.Headers.Add("Authorization", "invalidkey");
            var response = await client.SendAsync(request);

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task ReturnsAuthorizedForValidApiKey()
        {
            var host = await CreateHostBuilder("/", endpoints =>
            {
                endpoints.MapPost("/", async context =>
                {
                    await context.Response.WriteAsync("Received");
                });
            }).StartAsync();

            var client = host.GetTestClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/");
            request.Headers.Add("Authorization", "supersecretkey");
            var response = await client.SendAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal("Received", await response.Content.ReadAsStringAsync());
        }
    }
}
