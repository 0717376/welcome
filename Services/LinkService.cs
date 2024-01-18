using System;
using System.Collections.Generic;
using welcomeApp.Models;

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
    }
}
