using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Рабочка_beta_1._0.Models;

namespace Рабочка_beta_1._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly IjobsContext _context;
        public HomeController(IjobsContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index(string search, string category)
        {
            var jobs = await GetJobsAsync(search, category);

            var allCategories = await _context.Categories
                .Select(c => new {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Image,
                    JobCount = _context.Jobs.Count(j => j.CategoryId == c.Id)
                })
                .ToListAsync();

            ViewBag.PopularCategories = allCategories
                .Where(c => c.JobCount > 0)
                .OrderByDescending(c => c.JobCount)
                .Take(6)
                .ToList();

            ViewBag.AllCategories = allCategories;

            return View(jobs);
        }

        private async Task<IEnumerable<Job>> GetJobsAsync(string search, string category)
        {
            var query = _context.Jobs
               .Include(y => y.Category)
               .Include(e => e.Employer)
               .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(j => j.Title.Contains(search) ||
                j.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                if (int.TryParse(category, out int categoryId) && categoryId > 0)
                {
                    query = query.Where(j => j.CategoryId == categoryId);
                }
            }
            return await query.ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query, string category)
        {
            var jobs = await GetJobsAsync(query, category);
            return Json(jobs.Select(j => new
            {
                j.Id,
                j.Title,
                j.Location,
                j.Description,
                j.Salary,
                j.Views,
                j.CreatedAt
            }));
        }
    }
}