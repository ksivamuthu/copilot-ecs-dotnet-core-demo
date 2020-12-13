using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    services.AddTransient<DynamoDBContext>(c => new DynamoDBContext(c.GetService<IAmazonDynamoDB>(), config));
 
    services.AddSingleton<CoffeeService>();   
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

void SeedData(CoffeeService coffeeService)
{
    var coffees = new List<Coffee> {
        new Coffee() { CoffeeId = "cappucino", CoffeeName = "Cappucino" },
        new Coffee() { CoffeeId = "latte", CoffeeName = "Latte" },
        new Coffee() { CoffeeId = "mocha", CoffeeName = "Mocha" },
        new Coffee() { CoffeeId = "americano", CoffeeName = "Americano" },
        new Coffee() { CoffeeId = "macchiato", CoffeeName = "Macchiato" },
        new Coffee() { CoffeeId = "frappe", CoffeeName = "Frappe" },
        new Coffee() { CoffeeId = "corretto", CoffeeName = "Corretto" },
        new Coffee() { CoffeeId = "affogato", CoffeeName = "Affogato" },
        new Coffee() { CoffeeId = "filtercoffee", CoffeeName = "Filter Coffee" },
    };

    
    Task.Run(async () => {
        var existingCoffees = await coffeeService.GetAll();
        if(existingCoffees != null && existingCoffees.Count() > 0) {
            return;
        }
        foreach (var coffee in coffees)
        {
            await coffeeService.Create(coffee);    
        }        
    }).Wait();
}

void ConfigureApp(IApplicationBuilder app)
{
    SeedData(app.ApplicationServices.GetService<CoffeeService>());

    app.UsePathBase(new PathString("/coffee-service"));
    app.UseDeveloperExceptionPage();
    
    app.UseForwardedHeaders();
    app.UseRouting();
    app.UseCors();
    app.UseEndpoints(e => {
        e.MapGet("/", c => c.Response.WriteAsync("I'm coffee service!"));
        e.MapHealthChecks("/healthz", new HealthCheckOptions());
        e.MapControllers();
    });
}