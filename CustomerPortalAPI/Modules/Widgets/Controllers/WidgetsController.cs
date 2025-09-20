using Microsoft.AspNetCore.Mvc;

namespace CustomerPortalAPI.Modules.Widgets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WidgetsController : ControllerBase
    {
        /// <summary>
        /// Get all dashboard widgets
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetWidgets()
        {
            try
            {
                var widgets = new object[]
                {
                    new { id = 1, name = "Audit Summary", type = "chart", data = new { audits = 25, completed = 20 } },
                    new { id = 2, name = "Certificate Status", type = "gauge", data = new { active = 80, expiring = 12 } },
                    new { id = 3, name = "Action Items", type = "list", data = new { total = 67, overdue = 5 } },
                    new { id = 4, name = "Compliance Score", type = "meter", data = new { score = 94.5 } }
                };
                
                return Ok(widgets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get widget by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetWidget(int id)
        {
            try
            {
                var widget = new { id, name = $"Widget {id}", type = "generic", data = new { placeholder = true } };
                return Ok(widget);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}