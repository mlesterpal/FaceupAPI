using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Faceup.Models.Dto;
using Faceup.Services;

namespace Faceup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult AddNewUser(CreateUser user)
        {
            try
            {
                _userService.AddUser(user);
                return Ok(new
                {
                    message = "User added successfully",
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding user: {ex.Message}");
            }
        }
    }
}
