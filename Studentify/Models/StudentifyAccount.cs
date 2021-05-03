using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Models.Authentication;

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
        
        public List<Event> Events { get; set; }
    }
}
