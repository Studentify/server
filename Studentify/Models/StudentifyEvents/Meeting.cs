using System.Collections.Generic;

namespace Studentify.Models.StudentifyEvents
{
    public class Meeting : StudentifyEvent
    {
        public List<StudentifyAccount> Participants { get; set; }
        public int MaxNumberOfParticipants { get; set; }
    }
}