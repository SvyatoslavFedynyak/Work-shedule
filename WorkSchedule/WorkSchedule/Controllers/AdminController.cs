using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkSchedule.Data;
using WorkSchedule.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkSchedule.Controllers
{
    public class AdminController : Controller
    {
        private readonly ScheduleContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ScheduleContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        async public Task<IActionResult> Index()
        {
            List<IdentityUser> users = new List<IdentityUser>();
            users = _userManager.Users.ToList() ?? new List<IdentityUser>();
            return View(users);
        }
        [HttpGet]
        async public Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.AddToRoleAsync(user, "User");

            AdminView v = new AdminView();

            v.User = user;
            v.ApplicationRoles = _roleManager.Roles.Select(x => x.Name).ToList() ?? new List<string>();
            var roles = await _userManager.GetRolesAsync(user);
            v.RolesEnumerable = roles.ToList() ?? new List<string>();
            var claims = await _userManager.GetClaimsAsync(user);
            v.ClaimsEnumerable = new List<ClaimView>();
            var Claims = claims.ToList() ?? new List<Claim>();
            for (int i = 0; i < Claims.Count; i++)
            {
                v.ClaimsEnumerable.Add(new ClaimView() { Type = Claims[i].Type, Value = Claims[i].Value });
            }

            v.Claims = JsonConvert.SerializeObject(v.ClaimsEnumerable, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            v.Roles = JsonConvert.SerializeObject(v.Roles, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            v.ManagersEnumerable = managers.Select(x => x.Email).ToList() ?? new List<string>();
            v.Managers = JsonConvert.SerializeObject(v.ManagersEnumerable, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            return View(v);
        }
        [HttpPost]
        async public Task<IActionResult> Edit(AdminView model)
        {
            var Roles = model.RolesEnumerable;
            var Claims = model.ClaimsEnumerable;
            var user = await _userManager.FindByIdAsync(model.User.Id);
            var oldRoles = await _userManager.GetRolesAsync(user);
            var oldClaims = await _userManager.GetClaimsAsync(user);
            var OldRoles = oldRoles.ToList() ?? new List<string>();
            var OldClaims = oldClaims.ToList() ?? new List<Claim>();
            for (int i = 0; i < Roles.Count; i++)
            {
                await _userManager.AddToRoleAsync(user, Roles[i]);
            }
            for (int i = 0; i < OldRoles.Count; i++)
            {
                if (!Roles.Any(x => x == OldRoles[i]))
                {
                    await _userManager.RemoveFromRoleAsync(user, OldRoles[i]);
                }
            }
            for (int i = 0; i < Claims.Count; i++)
            {
                if (!oldClaims.Any(x => x.Type == Claims[i].Type && x.Value == Claims[i].Value))
                {
                    await _userManager.AddClaimAsync(user, new Claim(Claims[i].Type, Claims[i].Value));
                }
            }
            for (int i = 0; i < OldClaims.Count; i++)
            {
                if (!Claims.Any(x => x.Type == OldClaims[i].Type && x.Value == OldClaims[i].Value))
                {
                    await _userManager.RemoveClaimAsync(user, OldClaims[i]);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
