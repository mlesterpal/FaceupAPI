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

        public List<UserPostResponse> GetPostsByUserId(int? userId)
        {
            var param = new SqlParameter("@UserId", SqlDbType.Int)
            {
                Value = (userId is null or 0) ? DBNull.Value : userId.Value
            };

            return _context.Database
                .SqlQueryRaw<UserPostResponse>(
                    "EXEC dbo.usp_GetUserPosts @UserId",
                    param)
                .ToList();
        }

    }
}
