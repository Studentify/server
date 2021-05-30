using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Studentify.Models.Messages;

namespace Studentify.Models.HttpBody
{
    public class MessageDto
    {
        public string Content { get; set; }
        public int ThreadId { get; set; }
    }
}
