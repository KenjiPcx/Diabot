using CarbLoggerService.Models;
using CarbLoggerService.Services.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CarbLoggerService.Services.Concrete
{
    public class MealSchedulerService : IMealSchedulerService
    {
        private readonly Container _container;
        private const string _databaseName = "diabot-db";
        private const string _containerName = "schedules";

        public MealSchedulerService(CosmosClient client)
        {
            _container = client.GetContainer(_databaseName, _containerName);
        }

        public async Task<List<ScheduleDayBlock>> GetAllBlocks()
        {
            IOrderedQueryable<ScheduleDayBlock> queryable = _container.GetItemLinqQueryable<ScheduleDayBlock>();

            using FeedIterator<ScheduleDayBlock> linqFeed = queryable.ToFeedIterator();

            var blocks = new List<ScheduleDayBlock>();
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<ScheduleDayBlock> response = await linqFeed.ReadNextAsync();
                blocks.AddRange(response);
            }
            return blocks;
        }

        public async Task<ScheduleDayBlock> GetBlockById(string id)
        {
            var block = await _container.ReadItemAsync<ScheduleDayBlock>(id, PartitionKey.None);

            return block;
        }

        public async Task<ScheduleDayBlock> AddBlock(ScheduleDayBlock block)
        {
            block.BlockId = Guid.NewGuid();
            ScheduleDayBlock newBlock = await _container.CreateItemAsync(
                item: block
            );

            return newBlock;
        }

        public async Task<ScheduleDayBlock> UpdateBlock(string id, ScheduleDayBlock updatedBlock)
        {
            ScheduleDayBlock replacedBlock = await _container.ReplaceItemAsync(
                item: updatedBlock,
                id: id
            );

            return replacedBlock;
        }

        public async Task DeleteBlock(string id)
        {
            await _container.DeleteItemAsync<ScheduleDayBlock>(id, PartitionKey.None);
        }

        public async Task<ScheduleDayBlock> AddScheduleItemToBlock(ScheduleItem item, string blockId)
        {
            var block = await GetBlockById(blockId);
            block.ScheduleItems.Add(item);

            return await UpdateBlock(blockId, block);
        }

        public async Task<ScheduleDayBlock> RemoveScheduleItemFromBlock(ScheduleItem item, string blockId)
        {
            var block = await GetBlockById(blockId);
            block.ScheduleItems.Remove(item);

            return await UpdateBlock(blockId, block);
        }
    }
}
