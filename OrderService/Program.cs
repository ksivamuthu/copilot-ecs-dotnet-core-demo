using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Amazon.EventBridge;

WebHost.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Configure(ConfigureApp)
    .Build()
    .Run();

void ConfigureServices(IServiceCollection services)
{ 
     var config = new DynamoDBContextConfig  { 
        TableNamePrefix = $"{Environment.GetEnvironmentVariable("COPILOT_APPLICATION_NAME")}-{Environment.GetEnvironmentVariable("COPILOT_ENVIRONMENT_NAME")}-{Environment.GetEnvironmentVariable("COPILOT_SERVICE_NAME")}-"
    };
    services.AddAWSService<IAmazonDynamoDB>();
    services.AddTransient<IDynamoDBContext>(c => new DynamoDBContext(c.GetService<IAmazonDynamoDB>(), config));
 
    services.AddAWSService<IAmazonEventBridge>();
    services.AddSingleton<EventBusConfig>(c => new EventBusConfig(){ EventBusName =  $"{Environment.GetEnvironmentVariable("COPILOT_APPLICATION_NAME")}-{Environment.GetEnvironmentVariable("COPILOT_ENVIRONMENT_NAME")}-OrderEventBus"});

    services.AddSingleton<OrderService>();   
    services.AddControllers();
    services.AddHealthChecks();
    services.AddCors(options =>
        {           
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();                                        
                });
        });
}

void ConfigureApp(IApplicationBuilder app)
{
    app.UseDeveloperExceptionPage();
    app.UsePathBase(new PathString("/order-service"));
    
    app.UseForwardedHeaders();
    app.UseRouting();
    app.UseCors();
    app.UseEndpoints(e => {
        e.MapGet("/", c => c.Response.WriteAsync("I'm order service!"));
        e.MapHealthChecks("/healthz", new HealthCheckOptions());
        e.MapControllers();
    });
}

public class EventBusConfig 
{
    public string EventBusName { get; set; }
}