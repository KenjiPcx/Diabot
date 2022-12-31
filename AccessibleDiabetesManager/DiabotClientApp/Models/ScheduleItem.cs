using Newtonsoft.Json;

namespace Diabot.Models
{
    public class ScheduleItem
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ScheduleItemId { get; set; } = Guid.NewGuid();
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string MealName { get; set; }
        public List<string> MealIds { get; set; }
        public string Notes { get; set; }
        public Brush Background { get; set; }
    }
}
