using Microsoft.Extensions.Logging;
using System.Runtime;
using CommunityToolkit.Maui;
using Yoda.Config;
using Yoda.Services;
using Yoda.Services.Interfaces;
using Yoda.ViewModels;
using Yoda.Views;
using Yoda;

namespace Yoda
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.RegisterServices()
                .RegisterViewModels()
                .RegisterViews();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<IAiAssistant, IAiAssistant>();
            mauiAppBuilder.Services.AddTransient<ISettings, ConstantSettings>();

            // More services registered here.

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<QuestionViewModel>();
            mauiAppBuilder.Services.AddSingleton<AnswerViewModel>();

            // More view-models registered here.

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<YodaQuestionPage>();
            mauiAppBuilder.Services.AddSingleton<YodaAnswerPage>();

            // More views registered here.

            return mauiAppBuilder;
        }
    }
}
