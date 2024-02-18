using System;
using System.Collections.Generic;
using welcomeApp.Models;
using System.Text;
using System.Threading.Tasks;

namespace welcomeApp.Services
{
    public class LinkService
    {
        private readonly Dictionary<Guid, LinkItem> _links = new Dictionary<Guid, LinkItem>();
        private readonly OfficeLocationService _officeLocationService; // Добавлено
        private const int LinkDurationDays = 14; // Срок действия ссылки в днях

        // Добавлен конструктор для внедрения зависимости OfficeLocationService
        public LinkService(OfficeLocationService officeLocationService)
        {
            _officeLocationService = officeLocationService;
        }

        public async Task<LinkItem> CreateLink(LinkRequest request) // Изменено на асинхронный метод
        {
            // Получаем название города по региону
            if (request.WorkStart == null)
            {
                throw new ArgumentException("WorkStart cannot be null");
            }

            var cityName = await _officeLocationService.GetCityNameByRegionAsync(request.WorkStart.Region);

            // Получаем адрес офиса по названию города
            var officeAddress = await _officeLocationService.GetOfficeAddressByCityNameAsync(cityName);

            // Создаем элемент ссылки с добавленным адресом офиса
            var linkItem = new LinkItem
            {
                Guid = Guid.NewGuid(),
                Employee = request.Employee,
                Mentor = request.Mentor,
                Manager = request.Manager,
                WorkStart = request.WorkStart,
                ExpirationDate = DateTime.UtcNow.AddDays(LinkDurationDays)
            };

            // Если адрес найден, обновляем информацию об офисе в WorkStart
            if (!string.IsNullOrWhiteSpace(officeAddress))
            {
                linkItem.WorkStart.Office = new Office
                {
                    Address = officeAddress,
                    IsRegional = request.WorkStart.Office?.IsRegional ?? false
                };
            }

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
