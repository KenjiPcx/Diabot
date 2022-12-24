using Azure.Identity;
using CarbLoggerService.Services;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
        builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
        await InitDb(builder.Configuration);

        // Add internal services
        builder.Services.AddSingleton(config =>
        {
            var connectionstring = builder.Configuration["recipedb"];
            var client = new CosmosClient(connectionstring);

            return client;
        });

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IMealService, MealService>();

        var app = builder.Build();
        Debug.WriteLine(app.Configuration["RecipeDB"]);
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static async Task InitDb(IConfiguration configuration)
    {
        var connectionstring = configuration["recipedb"];
        var client = new CosmosClient(connectionstring);

        Database db = await client.CreateDatabaseIfNotExistsAsync(
                id: "diabot-db"
            );
        await db.CreateContainerIfNotExistsAsync(
            id: "meals",
            partitionKeyPath: "/meals",
            throughput: 400
        );
    }
}