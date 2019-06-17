using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkSchedule.Models.ViewModels
{
    public class AdminView
    {
        public IdentityUser User { get; set; }
        public string Claims { get; set; }
        public string Roles { get; set; }
        public List<ClaimView> ClaimsEnumerable { get; set; } = new List<ClaimView>();
        public List<string> RolesEnumerable { get; set; } = new List<string>();
        public List<string> ApplicationRoles { get; set; } = new List<string>();
        public List<string> ManagersEnumerable { get; set; } = new List<string>();
        public string Managers { get; set; }
            
    }
}