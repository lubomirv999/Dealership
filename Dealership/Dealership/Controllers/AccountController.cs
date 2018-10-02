namespace Dealership.Controllers
{
    using Dealership.Models;
    using Dealership.Models.AccountViewModels;
    using Dealership.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private const int PageSize = 6;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _usersService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration Configuration { get; }

        public AccountController(
            IAccountService usersService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersService = usersService;
            _roleManager = roleManager;
            Configuration = configuration;

            Task
                .Run(async () =>
                {
                    var roles = new[]
                       {
                            "Admin",
                            "Moderator"
                        };

                    foreach (var role in roles)
                    {
                        var roleExists = await _roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await _roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }
                    }

                    var adminEmail = "Admin@dealership.com";
                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            Email = adminEmail,
                            UserName = adminEmail
                        };

                        await _userManager.CreateAsync(adminUser, "Admin1234@");
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }).Wait();
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllCars", "Car");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "Moderator");
                if (result.Succeeded)
                {
                    return RedirectToAction("AllCars", "Car");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            this._usersService.Delete(id);

            return RedirectToAction("All");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("AllCars", "Car");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All(int page = 1)
        {
            var usersAndRoles = this._usersService.All(PageSize, page);
            usersAndRoles.CurrentPage = page;
            usersAndRoles.TotalPages = (int)Math.Ceiling(_usersService.Count() / (double)PageSize);

            return View(usersAndRoles);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            IList<string> userRoles = new List<string>();
            var user = _usersService.FindById(id);

            if (user == null)
            {
                TempData["roleResult"] = "Cannot find the user";
                return RedirectToAction(nameof(All));
            }

            userRoles = await _userManager.GetRolesAsync(user);

            return View("Details", new UserAndRolesModel() { User = user, Roles = userRoles, AllRoles = this._usersService.AllRoles() });
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(string userId, string adminRole, string moderatorRole)
        {
            ApplicationUser user = await this._userManager.FindByIdAsync(userId);

            bool isAdmin = String.IsNullOrEmpty(adminRole)
                ? false
                : await this._roleManager.RoleExistsAsync(adminRole);

            bool isModerator = String.IsNullOrEmpty(moderatorRole)
                ? false
                : await this._roleManager.RoleExistsAsync(moderatorRole);

            bool userExists = user != null;

            if (!userExists)
            {
                return RedirectToAction("All", new { page = 1 });
            }

            if (!isAdmin && !isModerator)
            {
                if (user.Email != "Admin@dealership.com")
                {
                    await this._userManager.RemoveFromRoleAsync(user, Configuration.GetSection("AdminRole").Value.ToString());
                }

                await this._userManager.RemoveFromRoleAsync(user, Configuration.GetSection("ModeratorRole").Value.ToString());

                return RedirectToAction("Details", new { id = user.Id });
            }

            if (isModerator)
                await this._userManager.AddToRoleAsync(user, moderatorRole);
            else
                await this._userManager.RemoveFromRoleAsync(user, Configuration.GetSection("ModeratorRole").Value.ToString());

            if (isAdmin)
                await this._userManager.AddToRoleAsync(user, adminRole);
            else if (user.UserName != "Admin@dealership.com")
                await this._userManager.RemoveFromRoleAsync(user, Configuration.GetSection("AdminRole").Value.ToString());          

            return RedirectToAction("Details", new { id = user.Id });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
