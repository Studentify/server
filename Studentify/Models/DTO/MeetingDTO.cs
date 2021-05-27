using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Models.HttpBody
{
    public class MeetingDTO : StudentifyEventDto
    {
        public int MaxNumberOfParticipants { get; set; }
    }
}