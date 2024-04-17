using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using LuxeLoft.Models;


namespace LuxeLoft.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public UserController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        public async Task<IActionResult> LoginPartial()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["UName"] = user?.Name;
            return PartialView("_LoginPartial");
        }

        public IActionResult SwitchMode(string mode)
        {
            // Validate input
            if (mode != "Buying" && mode != "Selling")
            {
                HttpContext.Session.SetString("UserMode", "Buying");
            }

            // Update the session variable
            HttpContext.Session.SetString("UserMode", mode);

            // Redirect to the home page or dashboard
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
