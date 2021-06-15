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
        public string Description { get; set; }

        public object Location => new
        {
            Address,
            Coordinates = new
            {
                Longitude = MapPoint.X,
                Latitude = MapPoint.Y,
            },
        };

        [JsonIgnore] public Point MapPoint { get; set; }
        [JsonIgnore] public Address Address { get; set; }

        public StudentifyAccount Author { get; set; }
        [Required] public int AuthorId { get; set; }
    }
}