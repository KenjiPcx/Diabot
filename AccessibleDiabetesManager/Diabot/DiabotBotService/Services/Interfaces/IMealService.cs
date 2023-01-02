using Diabot.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Diabot.Services.Interfaces
{
    public interface IMealService
    {
        Task<ObservableCollection<Meal>> GetAllMeals();
        Task<Meal> AddMeal(Meal meal);
        Task<Meal> UpdateMeal(string id, Meal updatedMeal);
        Task DeleteMeal(string id);
        Task<Meal> GetMealById(string id);
        Task<ObservableCollection<Meal>> GetMealsByIds(List<string> ids);
    }
}
