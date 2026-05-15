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
                UserId = 1,
                Message = message,
                ImageUrl = imageUrl
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public List<UserPostResponse> GetPostsByUserId(int userId)
        {
            return _context.Database
                .SqlQueryRaw<UserPostResponse>(
                    "EXEC dbo.usp_GetUserPosts @UserId",
                    new SqlParameter("@UserId", userId))
                .ToList();
        }
    }
}
