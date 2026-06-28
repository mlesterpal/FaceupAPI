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

    [HttpPatch("{notificationId}/read")]
    public IActionResult MarkAsRead(int notificationId, [FromQuery] int userId)
    {
        try
        {
            if (notificationId <= 0)
            {
                return BadRequest(new { message = "NotificationId must be greater than zero." });
            }

            if (userId <= 0)
            {
                return BadRequest(new { message = "UserId must be greater than zero." });
            }

            var result = _notificationService.MarkNotificationAsRead(notificationId, userId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error marking notification as read: {ex.Message}");
        }
    }
}
