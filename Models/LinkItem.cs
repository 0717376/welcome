using System;

namespace welcomeApp.Models
{
    public class LinkItem
    {
        public Guid Guid { get; set; } // Уникальный идентификатор для ссылки
        public string? Name { get; set; }
        public string? City { get; set; }
        public int? Region { get; set; }
        public DateTime ExpirationDate { get; set; } // Дата истечения срока действия ссылки
    }
}
