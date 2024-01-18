namespace welcomeApp.Models
{
    public class LinkRequest
    {
        public Employee? Employee { get; set; }
        public Mentor? Mentor { get; set; }
        public WorkStart? WorkStart { get; set; }
    }

    public class Employee
    {
        public string? Name { get; set; }
        public string? City { get; set; }
    }

    public class Mentor
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? MessengerLink { get; set; }
    }

    public class WorkStart
    {
        public DateTime StartDateTime { get; set; }
        public int Region { get; set; }
    }
}

