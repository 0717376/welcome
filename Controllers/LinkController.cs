using Microsoft.AspNetCore.Mvc;
using welcomeApp.Models;
using welcomeApp.Services;
using System;
using System.Text;
using System.Threading.Tasks; // Добавлено для поддержки асинхронности

namespace welcomeApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkController : Controller
    {
        private readonly LinkService _linkService;

        public LinkController(LinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpPost("generateUrl")]
        public async Task<IActionResult> GenerateUrl([FromBody] LinkRequest request) // Изменено на асинхронный метод
        {
            try
            {
                var linkItem = await _linkService.CreateLink(request); // Добавлено await
                var url = $"https://welcome.muravskiy.com/link/{linkItem.Guid}";
                return Ok(new { url = url });
            }
            catch (Exception ex)
            {
                // Здесь можно добавить более детальную обработку исключений
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{guid}")]
        public IActionResult GetLinkPage(Guid guid)
        {
            var linkItem = _linkService.GetLink(guid);
            if (linkItem == null)
            {
                return NotFound("The link is either invalid or has expired.");
            }

            return View("Index", linkItem);
        }
                [HttpGet("GetCalendarFile/{guid}")]
        
        public IActionResult GetCalendarFile(Guid guid)
        {
            var linkItem = _linkService.GetLink(guid);
            if (linkItem == null)
            {
                return NotFound("The link is either invalid or has expired.");
            }

            var calendarContent = _linkService.GenerateCalendarEvent(linkItem);
            var bytes = Encoding.UTF8.GetBytes(calendarContent);
            return File(bytes, "text/calendar", "event.ics");
        }
    }
}
