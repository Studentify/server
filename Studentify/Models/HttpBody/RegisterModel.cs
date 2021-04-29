using System.ComponentModel.DataAnnotations;

namespace Studentify.Models.HttpBody
{
    /// <summary>
    /// Data sent from client to server.
    /// Contains user credentials used
    /// to register new user to the application
    /// </summary>
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Lastname { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
