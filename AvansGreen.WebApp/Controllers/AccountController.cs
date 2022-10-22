using AvansGreen.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AvansGreen.WebApp.Controllers;

public class AccountController : Controller
{
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<IdentityUser> userMgr,
        SignInManager<IdentityUser> signInMgr, IConfiguration configuration)
    {
        _userManager = userMgr;
        _signInManager = signInMgr;
        _configuration = configuration;
    }

    //[AllowAnonymous]
    //public IActionResult Login(string returnUrl)
    //{
    //    return View(new LoginModel
    //    {
    //        ReturnUrl = returnUrl
    //    });
    //}

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                if ((await _signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false)).Succeeded)
                {
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var claims = claimsPrincipal.Claims.ToList();
                    //var currentUserVM = new CurrentUserViewModel() { Email = user.Email };
                    //if (claims.Any(x => x.Value == "Admin")) currentUserVM.TypeOfUser = TypeOfUser.Admin;
                    //else if (claims.Any(x => x.Value == "CanteenEmployee")) currentUserVM.TypeOfUser = TypeOfUser.CanteenEmployee;
                    //else if (claims.Any(x => x.Value == "Student")) currentUserVM.TypeOfUser = TypeOfUser.Student;

                    return RedirectToAction("Index", "Home");
                }
            }
        }
        ModelState.AddModelError("", "Invalid email or password");
        return View(loginModel);
    }

    public async Task<IActionResult> BrowseAnonymously()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("PacketOverview", "Packet");
    }


    public async Task<RedirectResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("~/Account/Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

}
