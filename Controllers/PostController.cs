using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Faceup.Models.Dto;
using Faceup.Services;
namespace Faceup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        public IActionResult AddNewPost(CreatePost post)
        {
            try
            {
                _postService.AddPost(post);
                return Ok(new
                {
                    message = "Post added successfully",
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding post: {ex.Message}");
            }
        }
    }
}
