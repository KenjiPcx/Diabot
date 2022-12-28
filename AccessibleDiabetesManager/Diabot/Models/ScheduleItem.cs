using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.Models
{
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
