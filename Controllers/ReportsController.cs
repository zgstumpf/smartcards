using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Authorization;

namespace assignment4.Controllers
{
    public class NewReport
    {
        public Guid SetId { get; set; }
    }

    [Authorize(Roles="Teacher")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            /* var applicationDbContext = _context.Report
                .Include(r => r.Set)
                .Include(r => r.ViewCount); */
            var reportset = await _context.Report
                .Include(r => r.Set)
                .Include(r => r.ViewCount)
                .ToListAsync();
            var ratings = await _context.SetRatingv3
                .ToListAsync();

            List<Report> reports = new();

            for (int i = 0; i < reportset.Count; i++)
            {
                Report report = new()
                {
                    ReportId = reportset[i].ReportId,
                    SetId = reportset[i].SetId,
                    Set = reportset[i].Set,
                    ViewCountId = reportset[i].ViewCountId,
                    ViewCount = reportset[i].ViewCount,
                    Ratings = ratings
                };
                report.CalculateAverageRating();
                reports.Add(report);
            }

            return View(reports);
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.ViewCount)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            
            if (report == null)
            {
                return NotFound();
            }

            // FlashcardSet set = GetSetById(report.SetId);
            FlashcardSet set = await _context.FlashcardSet
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.SetId == report.SetId);
            report.Set = set;

            return View(report);
        }

        private FlashcardSet GetSetById(Guid SetId)
        { 
            return _context.FlashcardSet.FirstOrDefault(s => s.SetId == SetId);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["SetId"] = new SelectList(GetNonDeletedSets(), "SetId", "Name");
            return View();
        }

        private List<FlashcardSet> GetNonDeletedSets()
        {
            return _context.FlashcardSet.Where(set => !set.IsDeleted).ToList();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid SetId)
        {
            Report report = new() { SetId = SetId };

            // Check if there is already a report for the given SetId
            if (ReportExists(report.SetId))
            {
                ViewData["ErrorMessage"] = "A report already exists for this set.";
                ViewData["SetId"] = new SelectList(GetNonDeletedSets(), "SetId", "Name", report.SetId);
                return View(report);
            }

            var set = await _context.FlashcardSet
                .FirstOrDefaultAsync(set => set.SetId == report.SetId);
            
            if (set == null)
            {
                ViewData["ErrorMessage"] = "This set does not exist.";
                ViewData["SetId"] = new SelectList(GetNonDeletedSets(), "SetId", "Name", report.SetId);
                return View(report);
            }

            if (set.IsDeleted == true)
            {
                ViewData["ErrorMessage"] = "Cannot create a report on a deleted set.";
                ViewData["SetId"] = new SelectList(GetNonDeletedSets(), "SetId", "Name", report.SetId);
                return View(report);
            }

            var ViewCount = await _context.ViewCount
                .FirstOrDefaultAsync(vc => vc.SetId == report.SetId);

            if (ViewCount == null)
            {
                ViewData["ErrorMessage"] = "Cannot create a report on a set without any views.";
                ViewData["SetId"] = new SelectList(GetNonDeletedSets(), "SetId", "Name", report.SetId);
                return View(report);
            }

            if (ModelState.IsValid)
            {
                report.ReportId = Guid.NewGuid();
                report.ViewCountId = ViewCount.ViewCountId;
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        /* public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["ViewCountId"] = new SelectList(_context.ViewCount, "ViewCountId", "ViewCountId", report.ViewCountId);
            return View(report);
        } */

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReportId,SetId,ViewCountId")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ViewCountId"] = new SelectList(_context.ViewCount, "ViewCountId", "ViewCountId", report.ViewCountId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.ViewCount)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }
        */

        //POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Report == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Report'  is null.");
            }
            var report = await _context.Report.FindAsync(id);
            if (report != null)
            {
                _context.Report.Remove(report);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 

        private bool ReportExists(Guid id)
        {
            return (_context.Report?.Any(e => e.SetId == id)).GetValueOrDefault();
        }
    }
}
