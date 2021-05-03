namespace Studentify.Models.StudentifyEvents
{
    public class Info : StudentifyEvent 
    {
        public InfoCategory Category { get; set; }
    }
    
    public enum InfoCategory {
        Warning,
        Alert,
        Notice
    }
}