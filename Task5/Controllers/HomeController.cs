using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Task5.Models;
using Task5.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Task5.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserContext db;
        public HomeController(UserContext context)
        {
            db = context;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            User usersearch = await db.Users.FirstOrDefaultAsync(p => p.Email == currentUserName);
            if (usersearch == null || usersearch.Status == false)
            {
                return RedirectToAction("Logout", "Account");
            }
            return View(db.Users.ToList());
        }
       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
