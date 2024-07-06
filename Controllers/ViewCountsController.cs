using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Authorization;

namespace assignment4.Controllers
{
    [Authorize]
    public class ViewCountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ViewCountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ViewCounts
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ViewCount.Include(v => v.Set);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ViewCounts/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ViewCount == null)
            {
                return NotFound();
            }

            var viewCount = await _context.ViewCount
                .Include(v => v.Set)
                .FirstOrDefaultAsync(m => m.ViewCountId == id);
            if (viewCount == null)
            {
                return NotFound();
            }

            return View(viewCount);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Student")]
        public IActionResult Increment([FromBody] BrowserView browser)
        {
            var FoundViewCount = _context.ViewCount
            .FirstOrDefault(m => m.SetId == browser.setId);
            if (FoundViewCount == null)
            {
                ViewCount NewVC = new();
                NewVC.ViewCountId = new Guid();
                NewVC.SetId = browser.setId;
                NewVC.Count = 1;
                _context.Add(NewVC);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine(FoundViewCount.Count);
                FoundViewCount.Count += 1;
                _context.SaveChanges();
            }
            return Content("");
        }

        private bool ViewCountExists(Guid id)
        {
          return (_context.ViewCount?.Any(e => e.ViewCountId == id)).GetValueOrDefault();
        }
    }
}
