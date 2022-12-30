using Diabot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.Services.Interfaces
{
    public interface ISchedulerService
    {
        Task<ObservableCollection<ScheduleItem>> GetAllScheduleItems();
        Task<ScheduleItem> GetScheduleItemById(string id);
        Task<ObservableCollection<ScheduleItem>> GetAllScheduleItemsBeforeDatetime(DateTime datetime);
        Task<ScheduleItem> UpdateScheduleItem(string id, ScheduleItem updatedItem);
        Task<ScheduleItem> AddScheduleItem(ScheduleItem newItem);
        Task DeleteScheduleItem(string id);
    }
}
