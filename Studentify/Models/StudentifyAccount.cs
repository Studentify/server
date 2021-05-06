using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Studentify.Models.Authentication;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Models
{
    /// <summary>
    /// Class that represents app user's account model in database.
    /// </summary>
    public class StudentifyAccount
    {
        [Key] public int Id { get; set; }
        [Required] public string StudentifyUserId { get; set; }
        [JsonIgnore] public StudentifyUser User { get; set; }
        [JsonIgnore] public List<StudentifyEvent> Events { get; set; }
    }
}
