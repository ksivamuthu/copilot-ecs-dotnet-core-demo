using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

WebHost.CreateDefaultBuilder(args)
    .ConfigureServices((s) => {})
    .Configure(ConfigureApp)
    .Build()
    .Run();

void ConfigureApp(IApplicationBuilder app)
{
    app.UseRouting();
    app.UseEndpoints(e => {
        e.MapGet("/", c => c.Response.WriteAsync("Hello world!"));
        e.MapGet("/hello/{name}", c => c.Response.WriteAsync($"Hello {c.Request.RouteValues["name"]}!"));
    });
}