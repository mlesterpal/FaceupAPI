using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Services;
using Microsoft.AspNetCore.Mvc;

namespace Faceup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}")]
    public IActionResult GetUser(int userId)
    {
        try
        {
            var user = _userService.GetUserProfile(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving user: {ex.Message}");
        }
    }

    [HttpPost("{userId}/profile-picture")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfilePicture(
        int userId,
        [FromForm] IFormFile image,
        CancellationToken cancellationToken)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest(new { message = "Image file is required." });
        }

        try
        {
            var profilePictureUrl = await _userService.UpdateProfilePictureAsync(
                userId,
                image,
                cancellationToken);

            return Ok(new UploadProfilePictureResponse { ProfilePictureUrl = profilePictureUrl });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error uploading profile picture: {ex.Message}");
        }
    }

    [HttpPut("{userId}/profile")]
    public IActionResult UpdateProfile(int userId, [FromBody] UpdateUserProfileRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Profile payload is required." });
        }

        try
        {
            var updatedUser = _userService.UpdateUserProfile(userId, request);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating user profile: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult AddNewUser([FromBody] CreateUser user)
    {
        try
        {
            _userService.AddUser(user);
            return Ok(new { message = "User added successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding user: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid password." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error logging in user: {ex.Message}");
        }
    }
}
