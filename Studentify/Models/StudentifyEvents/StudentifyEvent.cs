using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Studentify.Models.StudentifyEvents
{
    public abstract class StudentifyEvent
    {
        [Key] public int Id { get; set; }
        public string EventType => GetType().Name.ToUpper();
        [Required] public string Name { get; set; }
        [Required] public DateTime CreationDate { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
        [Required] public string Location { get; set; }
        public string Description { get; set; }

        [JsonIgnore, Required] public StudentifyAccount Author { get; set; }
        [Required] public int StudentifyAccountId { get; set; }
    }
}