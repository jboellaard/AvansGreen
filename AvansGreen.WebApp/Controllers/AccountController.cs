using AvansGreen.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvansGreen.WebApp.Controllers;

public class AccountController : Controller
{
    //private readonly UserManager<IdentityUser> userManager;
    //private readonly SignInManager<IdentityUser> signInManager;

    //public AccountController(UserManager<IdentityUser> userMgr,
    //    SignInManager<IdentityUser> signInMgr)
    //{
    //    userManager = userMgr;
    //    signInManager = signInMgr;

    //    //IdentitySeedData.EnsurePopulated(userMgr).Wait();
    //}

    public AccountController()
    {

    }

    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginModel
        {
            ReturnUrl = returnUrl
        });
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(new LoginModel());
    }

    [AllowAnonymous]
    public IActionResult PacketOverview()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult RegisterStudent()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        return View(loginModel);
    }

    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        //await signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }

    public async Task<IActionResult> AccessDenied(string returnUrl)
    {
        return View();
    }

}
