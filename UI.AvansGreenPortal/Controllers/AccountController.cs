using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.AvansGreenPortal.Models;
using UI_Infra.Security;

namespace UI.AvansGreenPortal.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AvansGreenUser> _userManager;
    private readonly SignInManager<AvansGreenUser> _signInManager;

    public AccountController(UserManager<AvansGreenUser> userMgr,
        SignInManager<AvansGreenUser> signInMgr)
    {
        _userManager = userMgr;
        _signInManager = signInMgr;
    }

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
            var user = await _userManager.FindByNameAsync(loginModel.Nr);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                if ((await _signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false)).Succeeded)
                {
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var claims = claimsPrincipal.Claims.ToList();
                    var currentUserVM = new CurrentUserViewModel() { Nr = user.UserName };
                    HttpContext.Session.SetString("UserFullName", user.FullName);

                    currentUserVM.IsAuthenticated = true;
                    currentUserVM.UserType = claims.FirstOrDefault(c => c.Type == "UserType").Value;

                    return RedirectToAction("Index", "Home", currentUserVM);
                }
            }
        }
        ModelState.AddModelError("", "Invalid email or password");
        return View(loginModel);
    }

    public IActionResult BrowseAnonymously()
    {
        LogoutUser();
        return RedirectToAction("PacketOverview", "Packet", new FilterPacketsViewModel() { CurrentUser = new CurrentUserViewModel() });
    }


    public IActionResult Logout()
    {
        LogoutUser();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public async void LogoutUser()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Clear();
    }

}
