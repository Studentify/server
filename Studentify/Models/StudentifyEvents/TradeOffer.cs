using System.ComponentModel.DataAnnotations;

namespace Studentify.Models.StudentifyEvents
{
    public class TradeOffer : StudentifyEvent 
    {
        [Required] public string Price { get; set; }
        [Required] public string Offer { get; set; }
        
        public int BuyerId { get; set; }
    }

}

