using System;

namespace Studentify.Models.StudentifyEvents
{
    public class TradeOffer : StudentifyEvent 
    {
        public string Price { get; set; }
        public string Offer { get; set; }
    }
}