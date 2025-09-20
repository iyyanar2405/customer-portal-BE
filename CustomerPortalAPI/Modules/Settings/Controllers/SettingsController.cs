using Microsoft.AspNetCore.Mvc;

namespace CustomerPortalAPI.Modules.Settings.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        /// <summary>
        /// Get all settings
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetSettings()
        {
            // TODO: Implement when Settings entities and repositories are created
            return Ok(new { message = "Settings module - coming soon" });
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        [HttpGet("{key}")]
        public async Task<ActionResult> GetSetting(string key)
        {
            // TODO: Implement when Settings entities and repositories are created
            return Ok(new { message = $"Setting {key} - coming soon" });
        }
    }
}