using Faceup.Models.Dto;
using Faceup.Services;
using Microsoft.AspNetCore.Mvc;

namespace Faceup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly FriendService _friendService;

    public FriendController(FriendService friendService)
    {
        _friendService = friendService;
    }

    [HttpGet]
    public IActionResult GetFriends([FromQuery] int userId)
    {
        try
        {
            var friends = _friendService.GetFriends(userId);
            return Ok(new { results = friends });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving friends: {ex.Message}");
        }
    }

    [HttpGet("incoming")]
    public IActionResult GetIncoming([FromQuery] int userId)
    {
        try
        {
            var requests = _friendService.GetIncomingFriendRequests(userId);
            return Ok(new { results = requests });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving incoming requests: {ex.Message}");
        }
    }

    [HttpGet("outgoing")]
    public IActionResult GetOutgoing([FromQuery] int userId)
    {
        try
        {
            var requests = _friendService.GetOutgoingFriendRequests(userId);
            return Ok(new { results = requests });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving outgoing requests: {ex.Message}");
        }
    }

    [HttpGet("suggestions")]
    public IActionResult GetSuggestions([FromQuery] int userId)
    {
        try
        {
            var users = _friendService.GetNonFriends(userId);
            return Ok(new { results = users });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving suggestions: {ex.Message}");
        }
    }

    [HttpPost("request")]
    public IActionResult SendRequest([FromQuery] int userId, [FromBody] SendFriendRequestDto dto)
    {
        try
        {
            var code = _friendService.SendFriendRequest(userId, dto.ReceiverId);
            var error = FriendService.GetErrorMessage(code);
            if (error != null)
            {
                return BadRequest(new { message = error, code });
            }

            return Ok(new { message = "Friend request sent successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending friend request: {ex.Message}");
        }
    }

    [HttpPost("accept/{friendshipId}")]
    public IActionResult Accept([FromQuery] int userId, int friendshipId)
    {
        return HandleWriteResult(_friendService.AcceptFriendRequest(friendshipId, userId), "Friend request accepted.");
    }

    [HttpPost("reject/{friendshipId}")]
    public IActionResult Reject([FromQuery] int userId, int friendshipId)
    {
        return HandleWriteResult(_friendService.RejectFriendRequest(friendshipId, userId), "Friend request rejected.");
    }

    [HttpDelete("request/{friendshipId}")]
    public IActionResult Cancel([FromQuery] int userId, int friendshipId)
    {
        return HandleWriteResult(_friendService.CancelFriendRequest(friendshipId, userId), "Friend request cancelled.");
    }

    [HttpDelete("{otherUserId}")]
    public IActionResult Unfriend([FromQuery] int userId, int otherUserId)
    {
        return HandleWriteResult(_friendService.RemoveFriend(userId, otherUserId), "Friend removed.");
    }

    private IActionResult HandleWriteResult(int code, string successMessage)
    {
        try
        {
            var error = FriendService.GetErrorMessage(code);
            if (error != null)
            {
                return BadRequest(new { message = error, code });
            }

            return Ok(new { message = successMessage });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
