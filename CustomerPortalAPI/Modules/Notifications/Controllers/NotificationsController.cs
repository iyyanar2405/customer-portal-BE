using Microsoft.AspNetCore.Mvc;

namespace CustomerPortalAPI.Modules.Notifications.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        /// <summary>
        /// Get all notifications
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetNotifications()
        {
            // TODO: Implement when Notifications entities and repositories are created
            return Ok(new { message = "Notifications module - coming soon" });
        }

        /// <summary>
        /// Get notification by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetNotification(int id)
        {
            // TODO: Implement when Notifications entities and repositories are created
            return Ok(new { message = $"Notification {id} - coming soon" });
        }
    }
}