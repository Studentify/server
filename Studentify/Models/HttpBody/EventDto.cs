using System;

namespace Studentify.Models.HttpBody
{
    public class EventDto
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
    }
}