using Infrastructure.Contexts;
using Infrastructure.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly WebAppContext _context;

    public AuthController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, WebAppContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [Route("/signup")]
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (!await _context.Users.AnyAsync(x => x.Email == viewModel.Email))
            {
                var userEntity = new UserEntity
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                };

                if((await _userManager.CreateAsync(userEntity, viewModel.Password)).Succeeded)
                {
                    if ((await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, false)).Succeeded)
                    {
                        return LocalRedirect("/");
                    }
                    else
                    {
                        return LocalRedirect("/signin");
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Something went wrong, could not create user";
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "User with the same email already exists";
            }
        }
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn(string returnUrl)
    {


        ViewData["ReturnUrl"] = returnUrl ?? "/";
        return View();
    }

    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel, string returnUrl)
    {
        if (ModelState.IsValid)
        {
            if((await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false)).Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
        }

        ViewData["ReturnUrl"] = returnUrl;
        ViewData["ErrorMessage"] = "Incorrect email or password";
        return View(viewModel);
    }

    [Route("/signout")]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
