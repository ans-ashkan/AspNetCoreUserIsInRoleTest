using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreUserIsInRoleTest.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> GetAll()
        {
            return Json(await _userManager.Users.Select(u => u.UserName).ToListAsync());
        }

        public async Task<IActionResult> New(string username, string password)
        {
            var result = await _userManager.CreateAsync(new ApplicationUser(username), password);
            if (result.Succeeded)
                return Ok();
            else
                return Json(result.Errors);
        }

        public async Task<IActionResult> Signin(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            if (result.Succeeded)
                return Ok();
            else
                return Content("invalid username or password");
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        public IActionResult Identity()
        {
            if (User.Identity.IsAuthenticated)
                return Json(User.Identity.Name);
            return Content("not authenticated");
        }

        public IActionResult IsInRole(string role)
        {
            return Json(new
            {
                IsInRole = User.IsInRole("admin"),
                IdentityName = User.Identity.Name
            });
        }

        public async Task<IActionResult> AddUserRole(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("user not found");

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
                return Ok();
            else
                return Json(result.Errors);
        }

        public async Task<IActionResult> RemoveUserRole(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("user not found");

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
                return Ok();
            else
                return Json(result.Errors);
        }
    }
}