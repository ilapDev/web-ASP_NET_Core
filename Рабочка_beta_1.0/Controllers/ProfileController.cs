using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Рабочка_beta_1._0.Models;

namespace Рабочка_beta_1._0.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IjobsContext _context;
        
        public ProfileController(IjobsContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task <IActionResult> Profile() 
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int currentUser = 0;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                currentUser = parsedUserId;

            }
            var userContext = await _context.Users
                .FirstOrDefaultAsync(job => job.Id == currentUser);
            var jobs = await _context.Jobs
                .Where(job => job.EmployerId == currentUser)
                .ToListAsync();
            ViewBag.Jobs = jobs;
            if (userContext == null)
            {
                return NotFound("Пользователь не найден");
            }
            return View(userContext);
        }
    }
}
