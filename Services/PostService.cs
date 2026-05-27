using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services
{
    public class PostService
    {
        private readonly PostRepository _postRepository;

        public PostService(PostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public void AddPost(string? message, string? imageUrl)
        {
            _postRepository.AddPost(message, imageUrl);
        }

        public List<UserPostResponse> GetUserPosts(int? userId)
        {
            return _postRepository.GetPostsByUserId(userId);
        }
    }
}
