using Diabot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.Services.Interfaces
{
    internal interface ISchedulerService
    {
        Task<List<Schedule>> GetAllSchedules();
        Task<Schedule> GetScheduleById(string id);
        Task<Schedule> UpdateSchedule(string id, Schedule schedule);
        Task<Schedule> AddSchedule(Schedule schedule);
        Task DeleteSchedule(string id);
        Task<Schedule> AddScheduleItemToSchedule(ScheduleItem item, string scheduleId);
        Task<Schedule> RemoveScheduleItemFromSchedule(ScheduleItem item, string scheduleId);
    }
}
