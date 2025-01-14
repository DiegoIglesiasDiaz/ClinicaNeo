using ClinicaNeo;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazor.Translations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Get the Web API URL from environment variables or GitHub Secrets
var webApiUrl = builder.Configuration["WEB_API_URL"] ?? "https://localhost:7063";  // Default to localhost for development

// Register HttpClient with the dynamic base URL
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(webApiUrl) });

builder.Services.AddMudServices();
builder.Services.AddMudTranslations();

var app = builder.Build();

await app.RunAsync();
