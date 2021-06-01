using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentify.Models.HttpBody;
using Studentify.Models;

namespace Studentify.IntegrationTests.GetDto
{
    class MeetingGetDto
    {
        public int Id { get; set; }
        public String EventType { get; set; }
        public String Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String Description { get; set; }
        public int AuthorId { get; set; }
        public List<StudentifyAccount> Participants { get; set; }
        public int MaxNumberOfParticipants { get; set; }
    }
}
