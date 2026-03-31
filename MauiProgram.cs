using Microsoft.Extensions.Logging;
using MonkeyFinder.View;

namespace MonkeyFinder;

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

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register app services, viewmodels and pages
        builder.Services.AddSingleton<MonkeyFinder.Services.MonkeyService>(); // register your service
        builder.Services.AddSingleton<MonkeyFinder.ViewModel.MonkeysViewModel>(); // register VM
        builder.Services.AddTransient<MonkeyFinder.View.MainPage>(); // register page
        builder.Services.AddTransient<MonkeyFinder.DetailsPage>(); 
        builder.Services.AddTransient<MonkeyFinder.ViewModel.MonkeyDetailsViewModel>();

        return builder.Build();
    }
}