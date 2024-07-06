using assignment4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace assignment4.Controllers
{
    // Navigating to localhost:XXXX/admin/create allows you to add new roles.
    // This class shouldn't be accessed during normal use of the web application.
    [Authorize(Roles="Delete this authorize statement if you ever want to add new roles.")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectRole role)
        {
            // Check to see if the role you typed in already exists.
            var roleExist = await roleManager.RoleExistsAsync(role.RoleName);
            
            // If it doesn't already exist, it is created.
            if (!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role.RoleName));
            }
            return View();
        }
    }
}
