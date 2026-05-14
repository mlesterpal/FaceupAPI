using Faceup.Models.Dto;
using Faceup.Repository;

namespace Faceup.Services


{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(CreateUser newUser)
        {
            _userRepository.AddUser(newUser);
        }   

    }
}
