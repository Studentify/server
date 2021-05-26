using Studentify.Models.StudentifyEvents;

namespace Studentify.Models.HttpBody
{
    public class InfoDto : EventDto
    {
        public InfoCategory Category { get; set; }
    }
}