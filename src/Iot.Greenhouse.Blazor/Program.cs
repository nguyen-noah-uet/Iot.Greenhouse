using System;
using System.Threading.Tasks;
using Iot.Greenhouse.Data;
using Iot.Greenhouse.EntityFrameworkCore;
using Iot.Greenhouse.Mqtt;
using Iot.Greenhouse.Nodes;
using Iot.Greenhouse.Sensors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using MQTTnet;
using MQTTnet.Client;
using Serilog;
using Serilog.Events;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using Volo.Abp.Data;

namespace Iot.Greenhouse.Blazor;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Information()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<GreenhouseBlazorModule>();
            builder.Services.AddScoped<INodeService, NodeService>();
            builder.Services.AddScoped<ISensorService, SensorService>();
            builder.Services.AddSingleton<IMqttService, MqttService>((provider) =>
            {
                // Configure your MQTT client options here
                var mqttOptions = new MqttClientOptionsBuilder()
                    .WithClientId(builder.Configuration["Mqtt:ClientId"])
                    .WithTcpServer(builder.Configuration["Mqtt:Server"], builder.Configuration.GetValue<int>("Mqtt:Port"))

                    .WithCredentials(builder.Configuration["Mqtt:Username"], builder.Configuration["Mqtt:Password"])

                    .WithCleanSession()
                    .WithTimeout(TimeSpan.FromSeconds(30))
                    .Build();

                var service = new MqttService(mqttOptions);
                Log.Information("Connecting to MQTT Broker.");
                service.ConnectAsync().Wait();
                Log.Information("Connected to MQTT Broker.");
                var mqttSubscribeOptions = new MqttFactory().CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic(GreenhouseStrings.Topics.NodeStatus + "#"))
                    .WithTopicFilter(f => f.WithTopic(GreenhouseStrings.Topics.Sensors + "#"))
                    .WithTopicFilter(f => f.WithTopic(GreenhouseStrings.Topics.DeviceStatus + "#"))
                    .WithTopicFilter(f => f.WithTopic(GreenhouseStrings.Topics.Command + "#"))
                    .WithTopicFilter(f => f.WithTopic(GreenhouseStrings.Topics.Notifications + "#"))
                    .Build();
                service.SubscribeAsync(mqttSubscribeOptions).Wait();
                return service;
            });

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddHttpClient();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddHostedService<StoreDataBackgroundService>();
            var app = builder.Build();
            SyncfusionLicenseProvider.RegisterLicense("MzA0MTQ1NEAzMjM0MmUzMDJlMzBpcFJaU2VIZTZzN29IRE40a2M4Ull4cU1vUTVrME9EWEg3eUZ2SnpPZjc4PQ==");
            var _mqttService = app.Services.GetRequiredService<IMqttService>();
            await _mqttService.ConnectAsync();

            await app.InitializeApplicationAsync();
            var ctx = app.Services.GetRequiredService<GreenhouseDbContext>();
            string databaseName = "IotGreenhouse";
            try
            {
                await app.Services.GetRequiredService<GreenhouseDbMigrationService>().MigrateAsync();
                await app.Services.GetRequiredService<GreenhouseDataSeeder>().SeedAsync(new DataSeedContext());
            } catch(SqlException ex)
            {
                Log.Error(ex, "Error when migrating database");
            }
            
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
