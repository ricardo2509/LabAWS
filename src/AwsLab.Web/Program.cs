using AwsLab.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
        var configuration = sp.GetRequiredService<IConfiguration>();
        var apiBaseUrl = configuration["ApiBaseUrl"];

        var baseAddress = string.IsNullOrWhiteSpace(apiBaseUrl)
        ? new Uri(builder.HostEnvironment.BaseAddress)
        : new Uri(apiBaseUrl);

        return new HttpClient { BaseAddress = baseAddress };
});

await builder.Build().RunAsync();
