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

void ConfigureApp(IApplicationBuilder app)
{
    app.UsePathBase(new PathString("/coffee-service"));
    
    app.UseForwardedHeaders();
    app.UseRouting();
    app.UseCors();
    app.UseEndpoints(e => {
        e.MapGet("/", c => c.Response.WriteAsync("I'm coffee service!"));
        e.MapHealthChecks("/healthz", new HealthCheckOptions());
        e.MapControllers();
    });
}