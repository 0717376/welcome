using System;

namespace welcomeApp.Models
{
    public class LinkItem
    {
        public Guid Guid { get; set; }
        public Employee? Employee { get; set; }
        public Mentor? Mentor { get; set; }
        public Manager? Manager { get; set; }
        public WorkStart? WorkStart { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
