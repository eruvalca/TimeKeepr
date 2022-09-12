using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using TimeKeepr.NativeClients.Common;
using TimeKeepr.UI.Common;
using TimeKeepr.UI.Common.Providers;
using TimeKeepr.UI.Services;

namespace TimeKeepr.NativeClients
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif


            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7268/api/v1/") });

            builder.Services.AddScoped<IdentityClientService>();
            builder.Services.AddScoped<PtoEntriesClientService>();
            builder.Services.AddScoped<HolidaysClientService>();

            builder.Services.AddScoped<ILocalSecureStorage, NativeLocalSecureStorage>();

            builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

            return builder.Build();
        }
    }
}