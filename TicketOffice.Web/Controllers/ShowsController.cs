using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
using TicketOffice.Domain.Models.Shows;
using TicketOffice.Domain.Services;

namespace TicketOffice.Web.Controllers
{
    public class ShowsController : Controller
    {
        private readonly IShowService _showService;
        private readonly ITicketService _ticketService;
        private UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public ShowsController(IShowService showService, ITicketService ticketService, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _showService = showService;
            _ticketService = ticketService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrUpdateShow(Show data)
        {
            ShowViewModel result = null;

            if (data.Id.Equals(Guid.Empty))
                result = await _showService.AddShow(data.Name, data.ShowDate, data.Duration, data.TicketCount);
            else
                result = await _showService.UpdateShow(data.Id, data.Name, data.ShowDate, data.Duration, data.TicketCount);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Index(int page = 0, GridFilter filter = null)
        {
            return Json(await _showService.GetPage(filter, page));
        }
    }
}