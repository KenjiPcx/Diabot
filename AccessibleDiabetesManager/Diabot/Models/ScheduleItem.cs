using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.Models
{
    public class ScheduleItem
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ScheduleItemId { get; set; } = Guid.NewGuid();
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string MealName { get; set; }
        public ObservableCollection<Guid> MealIds { get; set; }
        public string Notes { get; set; }
        public Brush Background { get; set; }
    }
}
