using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ImageShareLikes.Data;
using AdsAuthentication.Data;
using Microsoft.AspNetCore.Authorization;

namespace ImageShareLikes.Web.Controllers
{
    public class AccountController : Controller
    {

        private string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }



        public IActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            AccountRepository accountRepo = new(_connectionString);
            accountRepo.Signup(user);
            return Redirect("/Account/Login");
        }


        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();


            AccountRepository accountRepo = new(_connectionString);
            bool isValid = accountRepo.Login(email, password);
            if (!isValid)
            {
                TempData["Message"] = "Invalid login";
                return Redirect("/account/login");
            }
            return Redirect("/Home/Index");
        }


        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/Home/Index");
        }
    }
}
