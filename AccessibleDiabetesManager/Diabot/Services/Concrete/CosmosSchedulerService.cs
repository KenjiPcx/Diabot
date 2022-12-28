using Diabot.Models;
using Diabot.Services.Interfaces;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabot.Services.Concrete
{
    class CosmosSchedulerService : ISchedulerService
    {
        private readonly Container _container;
        private const string _databaseName = "diabot-db";
        private const string _containerName = "schedules";

        public CosmosSchedulerService(CosmosClient client)
        {
            _container = client.GetContainer(_databaseName, _containerName);
        }

        public async Task<List<Schedule>> GetAllSchedules()
        {
            IOrderedQueryable<Schedule> queryable = _container.GetItemLinqQueryable<Schedule>();

            using FeedIterator<Schedule> linqFeed = queryable.ToFeedIterator();

            var schedules = new List<Schedule>();
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<Schedule> response = await linqFeed.ReadNextAsync();
                schedules.AddRange(response);
            }
            return schedules;
        }

        public async Task<Schedule> GetScheduleById(string id)
        {
            var schedule = await _container.ReadItemAsync<Schedule>(id, PartitionKey.None);

            return schedule;
        }

        public async Task<Schedule> AddSchedule(Schedule schedule)
        {
            Schedule newSchedule = await _container.CreateItemAsync(
                item: schedule
            );

            return newSchedule;
        }

        public async Task<Schedule> UpdateSchedule(string id, Schedule updatedSchedule)
        {
            updatedSchedule.ScheduleId = new Guid(id);
            Schedule replacedSchedule = await _container.ReplaceItemAsync(
                item: updatedSchedule,
                id: id
            );

            return replacedSchedule;
        }

        public async Task DeleteSchedule(string id)
        {
            await _container.DeleteItemAsync<Schedule>(id, PartitionKey.None);
        }

        public async Task<Schedule> AddScheduleItemToSchedule(ScheduleItem item, string scheduleId)
        {
            var schedule = await GetScheduleById(scheduleId);
            schedule.ScheduleItems.Add(item);

            return await UpdateSchedule(scheduleId, schedule);
        }

        public async Task<Schedule> RemoveScheduleItemFromSchedule(ScheduleItem item, string scheduleId)
        {
            var schedule = await GetScheduleById(scheduleId);
            schedule.ScheduleItems.Remove(item);

            return await UpdateSchedule(scheduleId, schedule);
        }
    }
}
