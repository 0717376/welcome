using System;
using System.Collections.Generic;
using welcomeApp.Models;
using System.Text;

namespace welcomeApp.Services
{
    public class LinkService
    {
        private readonly Dictionary<Guid, LinkItem> _links = new Dictionary<Guid, LinkItem>();
        private const int LinkDurationDays = 14; // Срок действия ссылки в днях

        public LinkItem CreateLink(LinkRequest request)
        {
            var linkItem = new LinkItem
            {
                Guid = Guid.NewGuid(),
                Employee = request.Employee,
                Mentor = request.Mentor,
                WorkStart = request.WorkStart,
                ExpirationDate = DateTime.UtcNow.AddDays(LinkDurationDays)
            };

            _links.Add(linkItem.Guid, linkItem);

            return linkItem;
        }

        // Метод для получения LinkItem по GUID. Возвращает null, если ссылка не найдена или истекла.
        public LinkItem? GetLink(Guid guid)
        {
            if (_links.TryGetValue(guid, out var linkItem))
            {
                if (linkItem.ExpirationDate > DateTime.UtcNow)
                {
                    return linkItem;
                }
            }

            return null;
        }
        // Метод генерации ics
        public string GenerateCalendarEvent(LinkItem linkItem)
        {
            if (linkItem?.WorkStart?.StartDateTime == null)
            {
                throw new ArgumentNullException(nameof(linkItem.WorkStart.StartDateTime), "StartDateTime не может быть null.");
            }

            var startDateTime = linkItem.WorkStart.StartDateTime;

            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"DTSTART:{startDateTime.ToUniversalTime():yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTEND:{startDateTime.AddHours(1).ToUniversalTime():yyyyMMddTHHmmssZ}");
            sb.AppendLine($"SUMMARY:Оформление документов в Европлан");
            sb.AppendLine("DESCRIPTION:Встреча для оформления документов нового сотрудника.");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            return sb.ToString();
        }
    }
}
