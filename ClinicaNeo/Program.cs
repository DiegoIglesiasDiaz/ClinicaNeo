using Blazored.LocalStorage;
using ClinicaNeo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazor.Translations;
using Radzen;
using System.Globalization;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Get the Web API URL from environment variables or GitHub Secrets
var webApiUrl = builder.Configuration["WEB_API_URL"] ?? "https://localhost:7080";  // Default to localhost for development

// Register HttpClient with the dynamic base URL
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(webApiUrl) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<Radzen.ThemeService>();

// Other services registration
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddMudTranslations();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("es-ES")
    };

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
var app = builder.Build();
await app.RunAsync();
