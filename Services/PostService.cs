using Faceup.Models.Dto;
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

        public void AddPost(CreatePost post)
        {
            _postRepository.AddPost(post);
        }
    }
}
