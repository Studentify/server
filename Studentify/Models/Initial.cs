using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Models
{
    public class Initial
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
