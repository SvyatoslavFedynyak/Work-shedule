using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkSchedule.Models;

namespace WorkSchedule.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _us;

        public HomeController(UserManager<IdentityUser> us)
        {
            _us = us;
        }

        async public Task<IActionResult> Index()
        {
            var user = await _us.FindByEmailAsync("kolosok@gmail.com");//pushu
            await _us.AddToRoleAsync(user, "Admin");
            await _us.AddToRoleAsync(user, "Manager");
                return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
