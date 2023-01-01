using Newtonsoft.Json;

namespace CarbLoggerService.Models
{
    public class ScheduleDayBlock
    {
        [JsonProperty("id")]
        public Guid BlockId { get; set; }
        [JsonProperty("date")]
        public DateOnly Date { get; set; }
        [JsonProperty("items")]
        public List<ScheduleItem> ScheduleItems { get; set; }
    }

    public class ScheduleItem
    {
        public string MealId { get; set; }
        public TimeOnly Time { get; set; }
        public override string ToString()
        {
            return $"Meal {MealId} at time {Time.ToShortTimeString()}";
        }
    }
}
