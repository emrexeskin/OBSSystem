using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Services;

namespace OBSSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogController : ControllerBase
    {
        private readonly ActivityLogService _activityLogService;

        public ActivityLogController(ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        // Kullanıcı etkinliğini kaydetmek için asenkron API endpoint'i
        [HttpPost("log")]
        public async Task<IActionResult> LogUserActivity([FromBody] LogActivityRequest request)
        {
            await _activityLogService.LogUserActivityAsync(
                request.UserId, 
                request.Action, 
                request.Description, 
                request.IpAddress
            );

            return Ok("Activity logged successfully.");
        }

        // Tüm aktiviteleri asenkron olarak listeleyen endpoint
        [HttpGet("all")]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _activityLogService.GetAllActivitiesAsync();
            return Ok(activities);
        }
    }

    public class LogActivityRequest
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
    }
}