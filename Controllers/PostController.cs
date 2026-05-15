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

        [HttpPost]
        public IActionResult AddNewPost([FromBody] CreatePost post)
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding post: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetUserPosts([FromQuery] int userId)
        {
            try
            {
                var posts = _postService.GetUserPosts(userId);
                return Ok(new
                {
                    results = posts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving posts: {ex.Message}");
            }
        }
    }
}
