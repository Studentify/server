using System;
using System.ComponentModel.DataAnnotations;

namespace Studentify.Models.Messaging
{
    /// <summary>
    /// Class that represents a message model in database.
    /// </summary>
    public class Message
    {
        [Key] 
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int TradeOfferId { get; set; }
        public DateTime SendDate { get; set; }
        public string Content { get; set; }
    }
}