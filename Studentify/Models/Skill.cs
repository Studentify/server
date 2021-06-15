using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Models
{
    public class Skill
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public int Rate { get; set; }
        [Required] public int OwnerId { get; set; }
        [JsonIgnore] public StudentifyAccount Owner { get; set; }
    }
}
