using Studentify.Models.StudentifyEvents;

namespace Studentify.Models.HttpBody
{
    public class InfoDto : StudentifyEventDto
    {
        public InfoCategory Category { get; set; }
    }
}