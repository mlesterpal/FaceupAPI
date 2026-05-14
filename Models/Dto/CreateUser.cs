namespace Faceup.Models.Dto
{
    public class CreateUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string Gender { get; set; }
    }
}
