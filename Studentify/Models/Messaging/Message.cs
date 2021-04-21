using System;
using System.ComponentModel.DataAnnotations;
using Studentify.Models.Authentication;

namespace Studentify.Models.Messaging
{
    /// <summary>
    /// Class that represents a message model in database.
    /// </summary>
    public class Message
    {
        [Key] 
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int SeceiverID { get; set; }
        public DateTime SendDate { get; set; }
        public string Content { get; set; }
        
        public virtual StudentifyUser Sender { get; set; }
        public virtual StudentifyUser Receiver { get; set; }

    }
}