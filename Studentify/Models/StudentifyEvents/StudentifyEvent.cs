using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace Studentify.Models.StudentifyEvents
{
    public abstract class StudentifyEvent
    {
        [Key] public int Id { get; set; }
        public string EventType => GetType().Name.ToUpper();
        [Required] public string Name { get; set; }
        [Required] public DateTime CreationDate { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
        [Required, JsonIgnore] public Point Location { get; set; }
        public string Description { get; set; }

        [JsonIgnore] public StudentifyAccount Author { get; set; }
        [Required] public int StudentifyAccountId { get; set; }
    }
}