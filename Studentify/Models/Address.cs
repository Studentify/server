using System.ComponentModel.DataAnnotations;

namespace Studentify.Models
{
    public class Address
    {
        [Key] public int Id { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
    }
}