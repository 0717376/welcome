using Microsoft.AspNetCore.Mvc;
using welcomeApp.Models;
using welcomeApp.Services;
using System;

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
        public IActionResult GenerateUrl([FromBody] LinkRequest request)
        {
            try
            {
                var linkItem = _linkService.CreateLink(request);
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

            return View("Index", linkItem); // Используем View "Index" с моделью linkItem
        }
    }
}
