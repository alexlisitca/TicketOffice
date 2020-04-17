using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TicketOffice.Web.Models;

namespace TicketOffice.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserId"] = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
                ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
                ViewData["IsInRoleAdmin"] = User.IsInRole("Admin");
            }
            else
            {
                ViewData["UserId"] = Guid.Empty.ToString();
                ViewData["IsAuthenticated"] = false;
                ViewData["IsInRoleAdmin"] = false;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
