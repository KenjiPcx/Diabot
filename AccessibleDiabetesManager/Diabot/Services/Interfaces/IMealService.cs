using Diabot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    }
}
