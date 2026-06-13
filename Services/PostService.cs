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

        public List<PostLikeUserResponse> GetPostLikes(int postId)
        {
            return _postRepository.GetPostLikes(postId);
        }

        public List<PostCommentUserResponse> GetPostComments(int postId)
        {
            return _postRepository.GetPostComments(postId);
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

        public TogglePostShareResponse TogglePostShare(int postId, int userId)
        {
            var result = _postRepository.TogglePostShare(postId, userId);

            return new TogglePostShareResponse
            {
                PostId = postId,
                UserId = userId,
                IsShared = result.Shared,
                ShareCount = result.ShareCount,
                Message = result.Shared ? "Post shared successfully." : "Post unshared successfully."
            };
        }

        public DeleteUserPostResponse DeleteUserPost(int postId, int userId)
        {
            var deleted = _postRepository.DeleteUserPost(postId, userId);

            return new DeleteUserPostResponse
            {
                PostId = postId,
                UserId = userId,
                Deleted = deleted,
                Message = "Post deleted successfully."
            };
        }


    }
}
