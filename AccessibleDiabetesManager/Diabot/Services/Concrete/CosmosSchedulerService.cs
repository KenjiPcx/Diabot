using Diabot.Models;
using Diabot.Services.Interfaces;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Diabot.Services.Concrete
{
    public class CosmosSchedulerService : ISchedulerService
    {
        private readonly Container _container;
        private const string _databaseName = "diabot-db";
        private const string _containerName = "schedules";

        public CosmosSchedulerService(CosmosClient client)
        {
            _container = client.GetContainer(_databaseName, _containerName);
        }

        public async Task<ObservableCollection<ScheduleItem>> GetAllScheduleItems()
        {
            IOrderedQueryable<ScheduleItem> queryable = _container.GetItemLinqQueryable<ScheduleItem>();

            using FeedIterator<ScheduleItem> linqFeed = queryable.ToFeedIterator();

            var schedules = new List<ScheduleItem>();
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<ScheduleItem> response = await linqFeed.ReadNextAsync();
                schedules.AddRange(response);
            }
            return new ObservableCollection<ScheduleItem>(schedules);
        }

        public async Task<ScheduleItem> GetScheduleItemById(string id)
        {
            var schedule = await _container.ReadItemAsync<ScheduleItem>(id, PartitionKey.None);

            return schedule;
        }

        public async Task<ScheduleItem> AddScheduleItem(ScheduleItem schedule)
        {
            ScheduleItem newScheduleItem = await _container.CreateItemAsync(
                item: schedule
            );

            return newScheduleItem;
        }

        public async Task<ScheduleItem> UpdateScheduleItem(string id, ScheduleItem updatedScheduleItem)
        {
            updatedScheduleItem.ScheduleItemId = new Guid(id);
            ScheduleItem replacedScheduleItem = await _container.ReplaceItemAsync(
                item: updatedScheduleItem,
                id: id
            );

            return replacedScheduleItem;
        }

        public async Task DeleteScheduleItem(string id)
        {
            await _container.DeleteItemAsync<ScheduleItem>(id, PartitionKey.None);
        }
    }
}
