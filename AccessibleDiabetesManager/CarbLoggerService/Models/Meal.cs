using Newtonsoft.Json;

namespace CarbLoggerService.Models
{
    public class Meal
    {
        [JsonProperty("id")]
        public Guid MealId { get; set; }
        [JsonProperty("name")]
        public string MealName { get; set; }
        [JsonProperty("description")]
        public string MealDescription { get; set; }
        [JsonProperty("ingredients")]
        public List<Ingredient> Ingredients { get; set; }
        [JsonProperty("extraCarbsOffset")]
        public double ExtraCarbsOffset { get; set; }
    }

    public class Ingredient
    {
        public string IngredientName { get; set; }
        public CarbType CarbType { get; set; }
        public double CarbAmount { get; set; }
    }

    public enum CarbType
    {
        Slow,
        Medium,
        Fast
    }
}
