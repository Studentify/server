using System.ComponentModel.DataAnnotations;

namespace Studentify.Models.HttpBody
{
    public class TradeOfferDto: StudentifyEventDto
    {
        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; }
        
        [Required(ErrorMessage = "Offer is required")]
        public string Offer { get; set; }
        
        public int? BuyerId { get; set; }
    }
}