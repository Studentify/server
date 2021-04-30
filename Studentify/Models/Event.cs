using System;
using System.ComponentModel.DataAnnotations;

namespace Studentify.Models
{
    public class Event
    {
        [Key] public int Id { get; set; }
        public int EventType { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public StudentifyAccount Author { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}