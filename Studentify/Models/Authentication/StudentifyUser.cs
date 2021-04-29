using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Studentify.Models.Authentication
{
    /// <summary>
    /// Class that represents app user model in database.
    /// Can be extended with an additional attributes.
    /// </summary>
    public class StudentifyUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
