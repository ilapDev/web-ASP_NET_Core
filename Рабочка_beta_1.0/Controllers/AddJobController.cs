using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Рабочка_beta_1._0.Models;

namespace Рабочка_beta_1._0.Controllers
{
    public class AddJobController : Controller
    {
        private readonly IjobsContext _context;
        public AddJobController(IjobsContext context)
        {
            _context = context;
        }
        // GET: DetailJob
        public async Task<ActionResult> IndexAsync()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }
        
        // GET: DetailJob/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // POST: DetailJob/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Title, string Description, decimal Salary,  string Location, int CategoryId, string PaymentPeriod, string ContactPhone)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int currentUserId = int.Parse(userId.Value);
                

                ViewBag.PaytmentPeriods = new List<SelectListItem>
                {
                    new SelectListItem { Value = "hour", Text = "За час"},
                    new SelectListItem { Value = "day", Text = "За день" },
                    new SelectListItem { Value = "week", Text = "За неделю" },
                    new SelectListItem { Value = "month", Text = "За месяц" },
                    new SelectListItem { Value = "project", Text = "За проект" }
                };

                var job = new Job
                {
                    Title = Title,
                    Description = Description,
                    Salary = Salary,
                    EmployerId = currentUserId,
                    CreatedAt = DateTime.Now,
                    Views = 0,
                    Location = Location,
                    CategoryId = CategoryId,
                    ContactPhone = ContactPhone,
                    PaymentPeriod = PaymentPeriod,

                };
                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile", "Profile");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", $"Error : {ex.Message}");
                return View();
            }
        }
        
    }
}
