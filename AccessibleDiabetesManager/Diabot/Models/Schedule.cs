using Newtonsoft.Json;

namespace Diabot.Models
{
    /// <summary>
    /// A schedule for a day
    /// </summary>
    public class Schedule
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ScheduleId { get; set; } = Guid.NewGuid();
        public DateOnly Date { get; set; }
        public List<ScheduleItem> ScheduleItems { get; set; }
    }
}
