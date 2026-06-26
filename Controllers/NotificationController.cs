using Faceup.Services;
using Microsoft.AspNetCore.Mvc;

namespace Faceup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public IActionResult GetNotifications([FromQuery] int userId)
    {
        try
        {
            if (userId <= 0)
            {
                return BadRequest(new { message = "UserId must be greater than zero." });
            }

            var notifications = _notificationService.GetNotifications(userId);
            return Ok(new { results = notifications });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving notifications: {ex.Message}");
        }
    }
}
