using CarbLoggerService.Models;

namespace CarbLoggerService.Services.Interface
{
    public interface IMealSchedulerService
    {
        Task<List<ScheduleDayBlock>> GetAllBlocks();
        Task<ScheduleDayBlock> GetBlockById(string id);
        Task<ScheduleDayBlock> UpdateBlock(string id, ScheduleDayBlock block);
        Task<ScheduleDayBlock> AddBlock(ScheduleDayBlock block);
        Task DeleteBlock(string id);
        Task<ScheduleDayBlock> AddScheduleItemToBlock(ScheduleItem item, string blockId);
        Task<ScheduleDayBlock> RemoveScheduleItemFromBlock(ScheduleItem item, string blockId);
    }
}
