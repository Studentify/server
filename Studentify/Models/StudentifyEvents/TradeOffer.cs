using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Studentify.Models.StudentifyEvents
{
    public class TradeOffer : StudentifyEvent 
    {
        [Required] public string Price { get; set; }
        [Required] public string Offer { get; set; }

        [JsonIgnore] public StudentifyAccount Buyer { get; set; }
        public int? BuyerId { get; set; }
    }

}

