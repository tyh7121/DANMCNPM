using CrawlerProject_Web.Models.DTO;
using CrawlerProject_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CrawlerProject_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            var response = await _authService.LoginAsync(obj);
            if (response.User != null && !string.IsNullOrEmpty(response.Token))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, response.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, response.User.Role));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString("JWTToken", response.Token);
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Error: Something Wrong!";
            ModelState.AddModelError("Message", "Wrong username or password!");
            return View(obj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO obj)
        {
            var userDTO = await _authService.RegisterAsync(obj);
            if (userDTO.Id != 0)
            {
                TempData["success"] = "Your account has been successfully created.";
                return RedirectToAction("Login");
            }
            TempData["error"] = "Error: Something Wrong!";
            ModelState.AddModelError("Message", "Username already exists!");

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWTToken", "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
