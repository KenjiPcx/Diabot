using Diabot.Models;
using Diabot.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Diabot.Services.Concrete
{
    public class CosmosMealService : IMealService
    {
        private readonly Container _container;
        private const string _databaseName = "diabot-db";
        private const string _containerName = "meals";

        public CosmosMealService(CosmosClient client)
        {
            _container = client.GetContainer(_databaseName, _containerName);
        }

        public async Task<List<Meal>> GetAllMeals()
        {
            IOrderedQueryable<Meal> queryable = _container.GetItemLinqQueryable<Meal>();

            using FeedIterator<Meal> linqFeed = queryable.ToFeedIterator();

            var meals = new List<Meal>();
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<Meal> response = await linqFeed.ReadNextAsync();
                meals.AddRange(response);
            }
            return meals;
        }

        public async Task<Meal> GetMealById(string id)
        {
            var meal = await _container.ReadItemAsync<Meal>(id, PartitionKey.None);

            return meal;
        }

        public async Task<Meal> AddMeal(Meal meal)
        {
            Meal createdMeal = await _container.CreateItemAsync(
                item: meal
            );

            return createdMeal;
        }

        public async Task<Meal> UpdateMeal(string id, Meal updatedMeal)
        {
            updatedMeal.MealId = new Guid(id);
            Meal replacedMeal = await _container.ReplaceItemAsync(
                item: updatedMeal,
                id: id
            );

            return replacedMeal;
        }

        public async Task DeleteMeal(string id)
        {
            await _container.DeleteItemAsync<Meal>(id, PartitionKey.None);
        }
    }
}
