using Diabot.Services.Concrete;
using Diabot.Services.Interfaces;
using Diabot.ViewModels;
using Diabot.Views;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Diabot
{
    public static class MauiProgram
    {
        private const string _cosmosEndpoint = "https://diabetes-recipes-db.documents.azure.com:443/";
        private const string _cosmosKey = "zB9DVbgkFWw5E5nORf1J9a0FTXBTTUX2TPPg7L2HUHM4wwHxOdlvvubsuXukI8l6nbTB9qUTolgYACDb1MbnTw==";

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
            
            Task.Run(InitCosmosDb);
            builder = RegisterViews(builder);
            builder = RegisterViewModels(builder);
            builder = RegisterServices(builder);
            builder = RegisterOthers(builder);
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        private static MauiAppBuilder RegisterViews(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MealsPage>();
            builder.Services.AddTransient<MealDetailsPage>();
            builder.Services.AddTransient<AddMealPage>();
            builder.Services.AddTransient<EditMealPage>();
            return builder;
        }
        
        private static MauiAppBuilder RegisterViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MealsViewModel>();
            builder.Services.AddTransient<MealDetailsViewModel>();
            builder.Services.AddTransient<AddMealViewModel>();
            builder.Services.AddTransient<EditMealViewModel>();
            builder.Services.AddSingleton<SchedulesViewModel>();
            return builder;
        }
        
        private static MauiAppBuilder RegisterServices(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMealService, CosmosMealService>();
            builder.Services.AddSingleton<ISchedulerService, CosmosSchedulerService>();
            return builder;
        }

        private static MauiAppBuilder RegisterOthers(MauiAppBuilder builder)
        {
            // Cosmos DB
            builder.Services.AddSingleton(config =>
            {
                var client = new CosmosClient(_cosmosEndpoint, _cosmosKey);
                return client;
            });
            builder.Services.AddSingleton(Connectivity.Current);
            return builder;
        }

        private static async Task InitCosmosDb()
        {
            var client = new CosmosClient(_cosmosEndpoint, _cosmosKey);
            try
            {
                Database db = await client.CreateDatabaseIfNotExistsAsync(
                        id: "diabot-db"
                    );
                await db.CreateContainerIfNotExistsAsync(
                    id: "meals",
                    partitionKeyPath: "/meals",
                    throughput: 400
                );
                await db.CreateContainerIfNotExistsAsync(
                    id: "schedules",
                    partitionKeyPath: "/schedules",
                    throughput: 400
                );
            } catch (Exception ex) { }
        }
    }
}