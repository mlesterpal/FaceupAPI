using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Faceup.Models.Dto;
using Faceup.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Faceup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly FileStorageService _fileStorage;

        public PostController(PostService postService, FileStorageService fileStorage)
        {
            _postService = postService;
            _fileStorage = fileStorage;
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddNewPost([FromForm] CreatePostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                if (userId <= 0)
                {
                    return Unauthorized(new { message = "Unauthorized." });
                }

                if (string.IsNullOrWhiteSpace(request.Message)
                    && (request.Image == null || request.Image.Length == 0))
                {
                    return BadRequest(new { message = "Post must include text or an image." });
                }

                var imageUrl = await _fileStorage.SaveImageAsync(request.Image, cancellationToken);
                _postService.AddPost(userId, request.Message, imageUrl);

                return Ok(new
                {
                    message = "Post added successfully",
                    imageUrl
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding post: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetUserPosts([FromQuery] int? userId = null, [FromQuery] int? viewerUserId = null)
        {
            try
            {
                var posts = _postService.GetUserPosts(userId, viewerUserId);
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

        [HttpGet("{postId}/likes")]
        public IActionResult GetPostLikes(int postId)
        {
            try
            {
                var likes = _postService.GetPostLikes(postId);
                return Ok(new
                {
                    results = likes
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving post likes: {ex.Message}");
            }
        }

        [HttpGet("{postId}/comments")]
        public IActionResult GetPostComments(int postId)
        {
            try
            {
                var comments = _postService.GetPostComments(postId);
                return Ok(new { results = comments });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving post comments: {ex.Message}");
            }
        }

        [HttpPost("{postId}/comments")]
        public IActionResult CreatePostComment(int postId, [FromBody] CreatePostCommentRequest request)
        {
            try
            {
                if (request.UserId <= 0)
                {
                    return BadRequest(new { message = "UserId must be greater than zero." });
                }

                if (string.IsNullOrWhiteSpace(request.Comment))
                {
                    return BadRequest(new { message = "Comment cannot be empty." });
                }

                var result = _postService.AddPostComment(postId, request.UserId, request.Comment.Trim());
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating post comment: {ex.Message}");
            }
        }

        [HttpPost("{postId}/like/toggle")]
        public IActionResult TogglePostLike(int postId, [FromBody] TogglePostLikeRequest request)
        {
            try
            {
                if (request.UserId <= 0)
                {
                    return BadRequest(new { message = "UserId must be greater than zero." });
                }

                var result = _postService.TogglePostLike(postId, request.UserId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error toggling post like: {ex.Message}");
            }
        }

        [HttpPost("{postId}/share/toggle")]
        public IActionResult TogglePostShare(int postId, [FromBody] TogglePostShareRequest request)
        {
            try
            {
                if (request.UserId <= 0)
                {
                    return BadRequest(new { message = "UserId must be greater than zero." });
                }

                var result = _postService.TogglePostShare(postId, request.UserId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error toggling post share: {ex.Message}");
            }
        }

        [HttpDelete("{postId}")]
        public IActionResult DeleteUserPost(int postId, [FromBody] DeleteUserPostRequest request)
        {
            try
            {
                if (request.UserId <= 0)
                {
                    return BadRequest(new { message = "UserId must be greater than zero." });
                }

                var result = _postService.DeleteUserPost(postId, request.UserId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting post: {ex.Message}");
            }
        }

        private int GetAuthenticatedUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(ClaimTypes.Name)
                ?? User.FindFirstValue("sub");

            return int.TryParse(idValue, out var userId) ? userId : 0;
        }
    }
}
