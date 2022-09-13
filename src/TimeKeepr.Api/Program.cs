using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using TimeKeepr.Application;
using TimeKeepr.Infrastructure;

var AllowedSpecificOrigins = "AllowedSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var azureAppConfigConnectionString = builder.Configuration["AppConfigConnectionString"];
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddAzureAppConfiguration(options =>
    {
        options.Connect(azureAppConfigConnectionString)
        .Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName);
    });
});

// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration, builder);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7134", "https://witty-beach-0f0d5910f.1.azurestaticapps.net")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowedSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
