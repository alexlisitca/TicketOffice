using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
using TicketOffice.Domain.Services;
using TicketOffice.Web.Models;

namespace TicketOffice.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShowService _showService;
        private readonly ITicketService _ticketService;
        private UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IShowService showService, ITicketService ticketService, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _showService = showService;
            _ticketService = ticketService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrUpdateShow(Show data)
        {
            bool result = false;

            if (data.Id.Equals(Guid.Empty))
                result = await _showService.AddShow(data.Name, data.ShowDate, data.Duration, data.TicketCount);
            else
                result = await _showService.UpdateShow(data.Id, data.Name, data.ShowDate, data.Duration, data.TicketCount);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> BookTicket(Guid showId)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            bool result = await _ticketService.BookTicket(showId, Guid.Parse(user.Id));

            if (!result)
                return BadRequest();

            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 0, GridFilter filter = null)
        {
            var showList = await _showService.GetPage(filter, page);

            return View(showList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
