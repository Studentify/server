using System;

namespace Studentify.Models.HttpBody
{
    public class EventDTO
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public int StudentifyAccountId { get; set; }    //todo remove and get this from token
    }
}