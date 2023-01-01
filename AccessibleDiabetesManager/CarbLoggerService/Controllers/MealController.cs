using CarbLoggerService.Models;
using CarbLoggerService.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarbLoggerService.Controllers
{
    [Route("api/meals")]
    [ApiController]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly ILogger<MealController> _logger;

        public MealController(IMealService mealService, ILogger<MealController> logger)
        {
            _mealService = mealService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllMeals()
        {
            var meals = await _mealService.GetAllMeals();
            return Ok(meals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Meal(string id)
        {
            var meal = await _mealService.GetMealById(id);
            return Ok(meal);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddMeal(Meal newMeal)
        {
            var meal = await _mealService.AddMeal(newMeal);
            return Created(new Uri($"/meal/{meal.MealId}", UriKind.Relative), meal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeal(string id, Meal updatedMeal)
        {
            var meal = await _mealService.UpdateMeal(id, updatedMeal);
            return Ok(new { Message = $"Updated meal {id}" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(string id)
        {
            try
            {
                await _mealService.DeleteMeal(id);
                return Ok(new { Message = $"Deleted meal {id}" });
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
