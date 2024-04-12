# Проект "Welcome Link Generator"

Проект "Welcome Link Generator" - это сервис для генерации уникальных ссылок с контентом для новых сотрудников компании. Сервис позволяет через API запрос получить персонализированную ссылку, которая содержит всю необходимую информацию для нового сотрудника перед началом работы.

## Функциональность

- Генерация уникальных ссылок с помощью API запроса
- Персонализация контента ссылки с учетом данных о сотруднике, наставнике, менеджере и дате начала работы
- Отображение информации о месте и времени оформления документов, контактах наставника и менеджера
- Подробное описание первого рабочего дня и процесса адаптации
- Возможность добавить событие оформления в календарь
- Часто задаваемые вопросы (FAQ) для нового сотрудника
- Интеграция с API Европлана для получения актуального адреса офиса по региону
- Срок действия ссылки - 14 дней

## Технологии

- ASP.NET
- C#
- Razor Pages
- Javascript

## Пример запроса

```http
POST https://welcome.muravskiy.com/link/generateUrl
Content-Type: application/json

{
   "Employee": {
       "Name": "Екатерина"
   },
   "Mentor": {
       "Name": "Александр",
       "Phone": "+7 (926) 123-4567",
       "MessengerLink": "https://clck.ru/39o2qB"
   },
   "Manager": {
       "Name": "Юлия",
       "Phone": "+7 (926) 123-4567"
   },
   "WorkStart": {
       "StartDateTime": "2024-01-20T09:00:00",
       "Region": 77
   }
}
```

## Контроллеры
- LinkController - основной контроллер для генерации ссылок и отображения страницы приветствия

## Сервисы
- LinkService - сервис для создания, хранения и получения сгенерированных ссылок
- OfficeLocationService - сервис для получения адреса офиса по коду региона через API Европлана

## Модели
- LinkItem - модель элемента ссылки, содержащая всю информацию для генерации страницы
- LinkRequest - модель запроса на создание новой ссылки
- Employee, Mentor, Manager, WorkStart, Office - дополнительные модели для хранения информации о сотруднике, наставнике, менеджере, дате начала работы и офисе

## Дальнейшее развитие
- Расширение возможностей персонализации контента
- Улучшение пользовательского интерфейса страницы приветствия
- Оптимизация производительности и времени отклика сервиса