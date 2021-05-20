using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Studentify.Models.Messages
{
    public class Message
    {
        [Key] public int Id { get; set; }
        [Required] public StudentifyAccount Author { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public string Content { get; set; }
        [Required] public bool IsViewed { get; set; }

        [JsonIgnore] public Thread Thread { get; set; }
        [Required] public int ThreadId { get; set; }
    }
}
