using Microsoft.Extensions.Logging;
using NashDecorIS.Services;
using NashDecorIS.ViewModels;
using NashDecorIS.Views;
using Microsoft.Extensions.DependencyInjection;

namespace NashDecorIS
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ПРАВИЛЬНАЯ регистрация HttpClient
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IAuthServices, AuthServices>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddEditPage>();

#if DEBUG
            builder.Logging.AddConsole();
#endif

            return builder.Build();
        }
    }
}