namespace Studentify.Models.HttpBody
{
    public class StudentifyAccountDto
    {
        // public string UserName { get; set; } //todo get to know how to properly update this in db
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}