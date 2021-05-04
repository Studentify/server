using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Models
{
    /// <summary>
    /// Class that represents app user's account model in database.
    /// </summary>
    public class StudentifyAccount
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string StudentifyUsername { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [JsonIgnore] public List<StudentifyEvent> Events { get; set; }
    }
}
