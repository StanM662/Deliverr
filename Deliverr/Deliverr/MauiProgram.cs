﻿using Microsoft.Extensions.Logging;
using Deliverr.ViewModels;
using Deliverr.Views;


namespace Deliverr;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<BestellingPagina>();
        builder.Services.AddSingleton<NaamLogin>();
        builder.Services.AddSingleton<WachtwoordLogin>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<CompletePagina>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
