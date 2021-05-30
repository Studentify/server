using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Studentify.Models.StudentifyEvents
{
    public class Meeting : StudentifyEvent
    {
        public List<StudentifyAccount> Participants { get; set; }
        public int MaxNumberOfParticipants { get; set; }
    }
}