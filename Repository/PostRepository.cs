using System.Data;
using Faceup.Models;
using Faceup.Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Repository
{
    public class PostRepository
    {
        private readonly FaceupContext _context;
        public PostRepository(FaceupContext context)
        {
            _context = context;
        }

        public void AddPost(string? message, string? imageUrl)
        {
            var post = new Post
            {
                UserId = 2,
                Message = message,
                ImageUrl = imageUrl
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public List<UserPostResponse> GetPostsByUserId(int? userId, int? viewerUserId)
        {
            var userIdParam = new SqlParameter("@UserId", SqlDbType.Int)
            {
                Value = (userId is null or 0) ? DBNull.Value : userId.Value
            };
            var viewerUserIdParam = new SqlParameter("@ViewerUserId", SqlDbType.Int)
            {
                Value = (viewerUserId is null or 0) ? DBNull.Value : viewerUserId.Value
            };

            return _context.Database
                .SqlQueryRaw<UserPostResponse>(
                    "EXEC dbo.usp_GetUserPosts @UserId, @ViewerUserId",
                    userIdParam,
                    viewerUserIdParam)
                .ToList();
        }

        public (bool Liked, int LikeCount) TogglePostLike(int postId, int userId)
        {
            if (!_context.Posts.Any(p => p.Id == postId))
            {
                throw new KeyNotFoundException("Post not found.");
            }

            if (!_context.Users.Any(u => u.Id == userId))
            {
                throw new KeyNotFoundException("User not found.");
            }

            var postIdParam = new SqlParameter("@PostId", postId);
            var userIdParam = new SqlParameter("@UserId", userId);

            var likeExists = _context.Database
                .SqlQueryRaw<int>(
                    "SELECT COUNT(1) FROM dbo.Likes WHERE PostId = @PostId AND UserId = @UserId",
                    postIdParam,
                    userIdParam)
                .AsEnumerable()
                .FirstOrDefault() > 0;

            if (likeExists)
            {
                _context.Database.ExecuteSqlRaw(
                    "DELETE FROM dbo.Likes WHERE PostId = @PostId AND UserId = @UserId",
                    postIdParam,
                    userIdParam);
            }
            else
            {
                _context.Database.ExecuteSqlRaw(
                    "INSERT INTO dbo.Likes (PostId, UserId) VALUES (@PostId, @UserId)",
                    postIdParam,
                    userIdParam);
            }

            var likeCount = _context.Database
                .SqlQueryRaw<int>(
                    "SELECT COUNT(1) FROM dbo.Likes WHERE PostId = @PostId",
                    postIdParam)
                .AsEnumerable()
                .FirstOrDefault();

            return (!likeExists, likeCount);
        }

    }
}
