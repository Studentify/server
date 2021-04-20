using System.ComponentModel.DataAnnotations;

namespace Studentify.Models.HttpBody
{
    /// <summary>
    /// Data sent from client to server.
    /// Contains user credentials used
    /// to log in to the application
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
