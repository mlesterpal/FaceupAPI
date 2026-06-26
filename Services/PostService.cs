using Faceup.Models.Dto;
using Faceup.Models.Response;
using Faceup.Repository;

namespace Faceup.Services
{
    public class PostService
    {
        private readonly PostRepository _postRepository;
        private readonly NotificationService _notificationService;

        public PostService(PostRepository postRepository, NotificationService notificationService)
        {
            _postRepository = postRepository;
            _notificationService = notificationService;
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

        public CreatePostCommentResponse AddPostComment(int postId, int userId, string comment)
        {
            _postRepository.AddPostComment(postId, userId, comment);
            var postOwnerUserId = _postRepository.GetPostOwnerUserId(postId);

            _notificationService.CreateNotification(new CreateNotificationRequest
            {
                RecipientUserId = postOwnerUserId,
                ActorUserId = userId,
                Type = "PostComment",
                RelatedEntityId = postId
            });

            return new CreatePostCommentResponse
            {
                Message = "Comment added successfully."
            };
        }

        public TogglePostLikeResponse TogglePostLike(int postId, int userId)
        {
            var result = _postRepository.TogglePostLike(postId, userId);

            if (result.Liked)
            {
                var postOwnerUserId = _postRepository.GetPostOwnerUserId(postId);
                _notificationService.CreateNotification(new CreateNotificationRequest
                {
                    RecipientUserId = postOwnerUserId,
                    ActorUserId = userId,
                    Type = "PostLike",
                    RelatedEntityType = "Post",
                    RelatedEntityId = postId
                });
            }

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

            if (result.Shared)
            {
                var postOwnerUserId = _postRepository.GetPostOwnerUserId(postId);
                _notificationService.CreateNotification(new CreateNotificationRequest
                {
                    RecipientUserId = postOwnerUserId,
                    ActorUserId = userId,
                    Type = "PostShare",
                    RelatedEntityId = postId
                });
            }

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
