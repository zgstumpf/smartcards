using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace assignment4.Controllers
{
    [Authorize]
    public class SetRatingv3Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string loggedInUserId;

        public SetRatingv3Controller(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            loggedInUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // Unused
        // GET: SetRatingv3
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SetRatingv3.Include(s => s.FlashcardSet);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: SetRatingv3/RatingsForSet?SetId=f51060b6-2a9e-4c6a-a4cb-137dda19811f
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> RatingsForSet(Guid? SetId)
        {
            var setRatings = await _context.SetRatingv3
                .Include(s => s.FlashcardSet)
                .Where(sr => sr.RatedSetId == SetId)
                .ToListAsync();

            foreach (var rating in setRatings)
            {
                var user = await _context.Users.FindAsync(rating.RatingUserId);
                if (user != null)
                {
                    rating.RatingUserName = user.UserName;
                }

                var set = await _context.FlashcardSet.FindAsync(rating.RatedSetId);
                if (set != null)
                {
                    rating.RatedSetName = set.Name;
                }
            }

            return View(setRatings);
        }

        // GET: SetRatingv3/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SetRatingv3 == null)
            {
                return NotFound();
            }


            var setRatingv3 = await _context.SetRatingv3
                .Include(s => s.FlashcardSet)
                .FirstOrDefaultAsync(m => m.SetRatingId == id);
            if (setRatingv3 == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(setRatingv3.RatingUserId);
            if (user != null)
            {
                setRatingv3.RatingUserName = user.UserName;
            }


            return View(setRatingv3);
        }

        // GET: SetRatingv3/Create
        [Authorize(Roles = "Teacher, Student")]
        public IActionResult Create()
        {
            // ViewData["RatedSetId"] = new SelectList(_context.FlashcardSet, "SetId", "SetId");
            ViewData["RatedSetId"] = new SelectList(FlashcardSetsController.GetNonDeletedSets(_context), "SetId", "Name");
            return View();
        }

        // POST: SetRatingv3/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Create([Bind("SetRatingId,RatingUserId,RatingUserName,RatedSetId,RatedSetName,Rating,Comment,RatedDate")] SetRatingv3 setRatingv3)
        {
            if (ModelState.IsValid)
            {
                setRatingv3.SetRatingId = Guid.NewGuid();
                setRatingv3.RatingUserId = loggedInUserId;

                setRatingv3.RatedDate = DateTime.Now;
                _context.Add(setRatingv3);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FlashcardSets");

            }

            ViewData["RatedSetId"] = new SelectList(_context.FlashcardSet, "SetId", "SetId", setRatingv3.RatedSetId);
            return View(setRatingv3);
        }

        // GET: SetRatingv3/Edit/5
        //[Authorize(Roles = "Teacher")]
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null || _context.SetRatingv3 == null)
        //    {
        //        return NotFound();
        //    }

        //    var setRatingv3 = await _context.SetRatingv3.FindAsync(id);
        //    if (setRatingv3 == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RatedSetId"] = new SelectList(_context.FlashcardSet, "SetId", "SetId", setRatingv3.RatedSetId);
        //    return View(setRatingv3);
        //}

        // POST: SetRatingv3/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Teacher")]
        //public async Task<IActionResult> Edit(Guid id, [Bind("SetRatingId,RatingUserId,RatingUserName,RatedSetId,RatedSetName,Rating,Comment,RatedDate")] SetRatingv3 setRatingv3)
        //{
        //    if (id != setRatingv3.SetRatingId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(setRatingv3);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SetRatingv3Exists(setRatingv3.SetRatingId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RatedSetId"] = new SelectList(_context.FlashcardSet, "SetId", "SetId", setRatingv3.RatedSetId);
        //    return View(setRatingv3);
        //}

        //// GET: SetRatingv3/Delete/5
        //[Authorize(Roles = "Teacher")]
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.SetRatingv3 == null)
        //    {
        //        return NotFound();
        //    }

        //    var setRatingv3 = await _context.SetRatingv3
        //        .Include(s => s.FlashcardSet)
        //        .FirstOrDefaultAsync(m => m.SetRatingId == id);
        //    if (setRatingv3 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(setRatingv3);
        //}

        //// POST: SetRatingv3/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Teacher")]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    if (_context.SetRatingv3 == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.SetRatingv3'  is null.");
        //    }
        //    var setRatingv3 = await _context.SetRatingv3.FindAsync(id);
        //    if (setRatingv3 != null)
        //    {
        //        _context.SetRatingv3.Remove(setRatingv3);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SetRatingv3Exists(Guid id)
        {
          return (_context.SetRatingv3?.Any(e => e.SetRatingId == id)).GetValueOrDefault();
        }


        //POST: SetRatingv3/AverageRating?SetId=f51060b6-2a9e-4c6a-a4cb-137dda19811f
        [HttpPost]
        [Authorize(Roles = "Teacher, Student")]
        public async Task<ActionResult> AverageRating(Guid SetId)
        {
            var ratings = await _context.SetRatingv3
                .Where(s => s.RatedSetId == SetId)
                .Select(s => s.Rating)
                .ToListAsync();

            var average = ratings.Average();

            return Content(average.ToString());
        }
    }
}
