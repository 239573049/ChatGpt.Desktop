using ChatGpt.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Services
        .AddScoped((service) => new HttpClient())
        .AddChatGpt()
        .AddI18nForWasmAsync(builder.HostEnvironment.BaseAddress + "wwwroot/i18n");

await builder.Build().RunAsync();
