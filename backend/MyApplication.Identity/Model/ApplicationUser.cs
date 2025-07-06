using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApplication.Identity.Model
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
    public class ApplicationUserWithRoles
    {
        public ApplicationUser? applicationUser { get; set; }
        public List<IdentityRole>? roles { get; set; }
    }
}
