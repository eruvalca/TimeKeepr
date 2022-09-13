using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimeKeepr.UI.Common;
using TimeKeepr.UI.Common.Providers;
using TimeKeepr.UI.Services;
using TimeKeepr.Web;
using TimeKeepr.Web.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (builder.HostEnvironment.Environment == "Development")
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7268/api/v1/") });
}
else
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7268/api/v1/") });
}

builder.Services.AddScoped<IdentityClientService>();
builder.Services.AddScoped<PtoEntriesClientService>();
builder.Services.AddScoped<HolidaysClientService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ILocalSecureStorage, BrowserLocalSecureStorage>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

await builder.Build().RunAsync();
