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

        public List<UserPostResponse> GetUserPosts(int? userId, int? viewerUserId)
        {
            return _postRepository.GetPostsByUserId(userId, viewerUserId);
        }

        public TogglePostLikeResponse TogglePostLike(int postId, int userId)
        {
            var result = _postRepository.TogglePostLike(postId, userId);

            return new TogglePostLikeResponse
            {
                PostId = postId,
                UserId = userId,
                Liked = result.Liked,
                LikeCount = result.LikeCount,
                Message = result.Liked ? "Post liked." : "Post unliked."
            };
        }
    }
}
