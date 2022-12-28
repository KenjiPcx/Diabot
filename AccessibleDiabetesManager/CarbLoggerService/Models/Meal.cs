using Newtonsoft.Json;

namespace CarbLoggerService.Models
{
    public class Meal
    {
        [JsonProperty("id")]
        public Guid MealId { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double ExtraCarbsOffset { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Ingredient
    {
        public string IngredientName { get; set; }
        public CarbType CarbType { get; set; }
        public double CarbAmount { get; set; }

        public string Details => $"{CarbAmount}g - {CarbType} carbs";
    }

    public enum CarbType
    {
        Slow,
        Medium,
        Fast
    }
}
