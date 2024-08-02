using Microsoft.EntityFrameworkCore;
using project_frej.Data;
using project_frej.Middleware;
using project_frej.Services;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SensorDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<List<WebSocket>>();
builder.Services.AddScoped<IWebSocketHandler, WebSocketHandler>();

builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();

builder.Services.AddScoped<AggregationService>();
builder.Services.AddHostedService<AggregationHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader();
        });
    });
}
else
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policyBuilder =>
        {
            policyBuilder.WithOrigins("https://thiberg.dev")
                         .WithMethods("GET")
                         .AllowAnyHeader();
        });
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseWebSockets();

if (app.Environment.IsProduction())
{
    app.UseMiddleware<ApiKeyPostMiddleware>();
}

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();

public partial class Program { }
