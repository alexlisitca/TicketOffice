using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Services;

namespace TicketOffice.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IShowService _showService;
        private readonly ITicketService _ticketService;
        private UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public TicketsController(IShowService showService, ITicketService ticketService, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _showService = showService;
            _ticketService = ticketService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BookTicket(Guid showId)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var result = await _ticketService.BookTicket(showId, Guid.Parse(user.Id));

            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UnbookTicket(Guid ticketId)
        {
            var result = await _ticketService.UnbookTicket(ticketId);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }
    }
}