using Studentify.Models.StudentifyEvents;

namespace Studentify.Models.HttpBody
{
    public class InfoDTO : EventDTO
    {
        public InfoCategory Category { get; set; }
    }
}