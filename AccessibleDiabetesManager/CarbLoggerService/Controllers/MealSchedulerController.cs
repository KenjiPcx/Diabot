using CarbLoggerService.Models;
using CarbLoggerService.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarbLoggerService.Controllers
{
    [Route("api/mealScheduler")]
    [ApiController]
    public class MealSchedulerController : Controller
    {
        private readonly IMealSchedulerService _mealSchedulerService;
        private readonly ILogger<MealSchedulerController> _logger;

        public MealSchedulerController(IMealSchedulerService mealSchedulerService, ILogger<MealSchedulerController> logger)
        {
            _mealSchedulerService = mealSchedulerService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllSchedules()
        {
            var blocks = await _mealSchedulerService.GetAllBlocks();
            return Ok(blocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Schedule(string id)
        {
            var block = await _mealSchedulerService.GetBlockById(id);
            return Ok(block);
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewSchedule(ScheduleDayBlock newBlock)
        {
            var block = await _mealSchedulerService.AddBlock(newBlock);
            return Created(new Uri($"/blocks/{block.BlockId}", UriKind.Relative), block);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, ScheduleDayBlock updatedBlock)
        {
            var block = await _mealSchedulerService.UpdateBlock(id, updatedBlock);
            return Ok(new { Message = $"Updated block {id}" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlock(string id)
        {
            try
            {
                await _mealSchedulerService.DeleteBlock(id);
                return Ok(new { Message = $"Deleted block {id}" });
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPut("addItemToSchedule{scheduleId}")]
        public async Task<IActionResult> AddItemToSchedule(string scheduleId, ScheduleItem item)
        {
            var block = await _mealSchedulerService.AddScheduleItemToBlock(item, scheduleId);
            return Ok(new { Message = $"Added item {item} into schedule {scheduleId}"});
        }

        [HttpPut("removeItemFromSchedule{scheduleId}")]
        public async Task<IActionResult> RemoveItemFromSchedule(string scheduleId, ScheduleItem item)
        {
            var block = await _mealSchedulerService.RemoveScheduleItemFromBlock(item, scheduleId);
            return Ok(new { Message = $"Removed item {item} from schedule {scheduleId}" });
        }
    }
}
