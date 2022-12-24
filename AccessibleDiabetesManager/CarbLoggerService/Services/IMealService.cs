using CarbLoggerService.Models;

namespace CarbLoggerService.Services
{
    public interface IMealService
    {
        Task<List<Meal>> GetAllMeals();
        Task<Meal> AddMeal(Meal meal);
        Task<Meal> UpdateMeal(string id, Meal updatedMeal);
        Task DeleteMeal(string id);
        Task<Meal> GetMealById(string id);
    }
}
