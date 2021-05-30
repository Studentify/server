using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Models.Messages
{
    public class Thread
    {
        [Key] public int Id { get; set; }
        [JsonIgnore] public StudentifyEvent ReferencedEvent { get; set; }
        [Required] public int ReferencedEventId { get; set; }
        [Required] public StudentifyAccount UserAccount { get; set; }
        [JsonIgnore] public List<Message> Messages { get; set; }
    }
}
