using Faceup.Models;
using Faceup.Models.Dto;

namespace Faceup.Repository

{
    public class UserRepository
    {
        private readonly FaceupContext _context;
        public UserRepository(FaceupContext context)
        {
            _context = context;
        }

        //CRUD operation for User

        public void AddUser(CreateUser newUser)
        {
            var user = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Password = newUser.Password,
                Email = newUser.Email,
                BirthDate = newUser.BirthDate,
                Gender = newUser.Gender
            };
            _context.Users.Add(user);
            _context.SaveChanges();
        }

    }
}
