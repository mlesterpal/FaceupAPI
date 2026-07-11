using Faceup.Models.Dto;
using Faceup.Services;
using Microsoft.AspNetCore.Mvc;

namespace Faceup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("conversations")]
    public IActionResult GetConversations([FromQuery] int userId)
    {
        try
        {
            var conversations = _messageService.GetConversations(userId);
            return Ok(new { results = conversations });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving conversations: {ex.Message}");
        }
    }

    [HttpGet("conversations/{conversationId}/messages")]
    public IActionResult GetConversationMessages(int conversationId, [FromQuery] int userId)
    {
        try
        {
            var messages = _messageService.GetConversationMessages(conversationId, userId);
            return Ok(new { results = messages });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving conversation messages: {ex.Message}");
        }
    }

    [HttpPost("send")]
    public IActionResult SendMessage([FromQuery] int userId, [FromBody] SendMessageRequest request)
    {
        try
        {
            var response = _messageService.SendMessage(userId, request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending message: {ex.Message}");
        }
    }

    [HttpPatch("conversations/{conversationId}/read")]
    public IActionResult MarkConversationRead(int conversationId, [FromQuery] int userId)
    {
        try
        {
            var response = _messageService.MarkConversationRead(conversationId, userId);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error marking conversation as read: {ex.Message}");
        }
    }

    [HttpGet("unread-count")]
    public IActionResult GetUnreadConversationCount([FromQuery] int userId)
    {
        try
        {
            var response = _messageService.GetUnreadConversationCount(userId);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving unread conversation count: {ex.Message}");
        }
    }
}
