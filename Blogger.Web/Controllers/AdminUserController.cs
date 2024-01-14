using Blogger.Web.Models.ViewModel;
using Blogger.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminUserController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository=userRepository;
            _userManager=userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var users = await _userRepository.GetAll();
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModel.User
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    EmailAddress = user.Email
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Show(UserViewModel request)
        {
            if (!ModelState.IsValid) return View(request);

            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

            if (identityResult != null)
            {
                if (identityResult.Succeeded)
                {
                    // assign role to this user
                    var roles = new List<string> { "User"};

                    if (request.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await _userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult != null && identityResult.Succeeded)
                    {
                        return RedirectToAction("Show", "AdminUser");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var identityResult = await _userManager.DeleteAsync(user);

                if (identityResult != null && identityResult.Succeeded)
                {
                    return RedirectToAction("Show", "AdminUser");
                }
            }

            return View();
        }
    }
}
