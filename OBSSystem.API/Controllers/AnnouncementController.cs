using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Services;
using OBSSystem.Core.Entities;

namespace OBSSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly AnnouncementService _announcementService;

        public AnnouncementController(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpPost]
        public IActionResult CreateAnnouncement([FromBody] Announcement announcement)
        {
            try
            {
                var userIdClaim = User.FindFirst("sub")?.Value;
                var roleClaim = User.FindFirst("role")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
                {
                    return Unauthorized(new { message = "User information is missing in the token." });
                }

                announcement.SenderID = int.Parse(userIdClaim);
                _announcementService.CreateAnnouncement(announcement);

                return Ok(new { message = "Announcement created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    

        [HttpGet]
        public IActionResult GetAnnouncements()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("sub")?.Value);
                var role = User.FindFirst("role")?.Value;
                var announcements = _announcementService.GetAnnouncementsForUser(userId, role);
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
