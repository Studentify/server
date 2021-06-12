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
        public string UserName => User.UserName;
        public string FirstName => User.FirstName;
        public string LastName => User.LastName;
        public string Email => User.Email;
        [JsonIgnore] public StudentifyUser User { get; set; }
        [JsonIgnore] public List<StudentifyEvent> Events { get; set; }
        [Required, JsonIgnore] public string StudentifyUserId { get; set; }
        [JsonIgnore] public List<Meeting> Meetings { get; set; }
        [JsonIgnore] public List<Skill> Skills { get; set; }
    }
}
