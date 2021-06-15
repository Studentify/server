using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Models.Messages
{
    public class Thread
    {
        [Key] public int Id { get; set; }
        public StudentifyEvent ReferencedEvent { get; set; }
        [Required] public int ReferencedEventId { get; set; }
        [Required] public StudentifyAccount UserAccount { get; set; }
        [NotMapped] public Message LastMessage { get; set; }
        [JsonIgnore] public List<Message> Messages { get; set; }
    }
}
