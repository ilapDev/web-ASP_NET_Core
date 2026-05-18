using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Рабочка_beta_1._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Рабочка_beta_1._0.Controllers
{
    public class AuthController : Controller
    {
        private readonly IjobsContext _context;
        public AuthController(IjobsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return Json(new {success = false, message = "Введите логин и пароль" });
            }
            var user = _context.Users
                .Include(r=>r.RoleNavigation)
                .FirstOrDefault(u=>u.Login == login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Json(new { success = false, message = "Неверный логин или пароль" });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.RoleNavigation.Title)
                
            };
            var identity = new ClaimsIdentity( claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );
            return Json(new { success = true});
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string Username, string Phone, string Adress, string Login, string Password)
        {   
            if (_context.Users.Any(u=>u.Login == Login))
                return Json(new { success = false, message = "Такой логин уже существует" });

            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password))
                return Json(new { success = false, message = "Заполните все обязательные поля!" });

            if (Password.Length < 6)
                return Json(new { success = false, message = "Пароль должен содержать минимум 6 символов" });
            if (Login.Length < 6)
                return Json(new { success = false, message = "Логин должен содержать минимум 6 символов"});


            var user = new User
            {
                Username = Username,
                Login = Login,
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
                Role = 1,
                CreatedAt = DateTime.Now,
                Phone = Phone,
                Address = Adress,
                Avatar = ""
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
