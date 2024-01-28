﻿using KackelboControl_App.Services;
using Microsoft.Extensions.Logging;

namespace KackelboControl_App
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

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddMauiBlazorWebView();            

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
