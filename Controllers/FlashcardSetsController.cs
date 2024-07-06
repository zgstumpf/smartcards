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
    public class FlashcardSetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string loggedInUserId;

        public FlashcardSetsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            loggedInUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: FlashcardSets
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Index()
        {
            var sets = await _context.FlashcardSet
                .Where(f => f.IsDeleted == false)
                .Include(f => f.User)
                .ToListAsync();
            var ratings = await _context.SetRatingv3
                .ToListAsync();
            var views = await _context.ViewCount
                .ToListAsync();

            List<SetVM> setVMs = new();

            for(int i = 0; i < sets.Count; i++)
            {
                SetVM VM = new() {
                    Set = sets[i],
                };
                var id = VM.Set.SetId;

                var rating = ratings
                    .Where(r => r.RatedSetId == id)
                    .Average(r => r.Rating);
                if (rating != null)
                {
                    // This abomination gets the average rating (double), rounds it up or down, converts to a string, then converts to an int
                    VM.Rating.Rating = int.Parse(Math.Round(rating.Value, MidpointRounding.ToEven).ToString());
                }

                VM.Views = views
                    .FirstOrDefault(vc => vc.SetId == id);

                setVMs.Add(VM);
            }
         
            return View(setVMs);
        }

        // GET: FlashcardSets/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.FlashcardSet == null)
            {
                return NotFound();
            }
            SetVM VM = new();

            VM.Set = await _context.FlashcardSet
                .Include(f => f.User)
                .Include(f => f.Flashcards.Where(f => f.SetId == id && f.IsDeleted == false))
                .FirstOrDefaultAsync(m => m.SetId == id);

            var rating = await _context.SetRatingv3
                .Where(sr => sr.RatedSetId == id)
                .AverageAsync(sr => sr.Rating);

            if(rating != null ) { 
                // This abomination gets the average rating (double), rounds it up or down, converts to a string, then converts to an int
                VM.Rating.Rating = int.Parse(Math.Round(rating.Value, MidpointRounding.ToEven).ToString());
            }

            VM.Views = await _context.ViewCount
                .FirstOrDefaultAsync(vc => vc.SetId == id);

            if (VM.Set == null)
            {
                return NotFound();
            }

            return View(VM);
        }

        // GET: FlashcardSets/Study/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Study(Guid? id)
        {
            if (id == null || _context.FlashcardSet == null)
            {
                return NotFound();
            }

            var flashcardSet = await _context.FlashcardSet
                .Include(f => f.User)
                .Include(f => f.Flashcards.Where(f => f.SetId == id && f.IsDeleted == false))
                .FirstOrDefaultAsync(m => m.SetId == id);
            if (flashcardSet == null)
            {
                return NotFound();
            }

            return View(flashcardSet);
        }

        // GET: FlashcardSets/Practice/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Practice(Guid? id)
        {
            if (id == null || _context.FlashcardSet == null)
            {
                return NotFound();
            }

            var flashcardSet = await _context.FlashcardSet
                .Include(f => f.User)
                .Include(f => f.Flashcards.Where(f => f.SetId == id && f.IsDeleted == false))
                .FirstOrDefaultAsync(m => m.SetId == id);
            if (flashcardSet == null)
            {
                return NotFound();
            }

            return View(flashcardSet);
        }

        // GET: FlashcardSets/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["User"] = new SelectList(_context.Users, "UserName", "UserName");
            return View();
        }

        // POST: FlashcardSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("SetId,Name,Description,Category,CreatedDate,UpdatedDate,IsDeleted")] FlashcardSet flashcardSet)
        {
            if (ModelState.IsValid)
            {
                flashcardSet.SetId = Guid.NewGuid();

                // Automatically set the set UserId to the UserId of the logged in user.
                flashcardSet.UserId = loggedInUserId;

                flashcardSet.CreatedDate = DateTime.Now;
                flashcardSet.UpdatedDate = DateTime.Now;

                flashcardSet.IsDeleted = false;

                _context.Add(flashcardSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", flashcardSet.UserId);
            return View(flashcardSet);
        }

        // GET: FlashcardSets/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.FlashcardSet == null)
            {
                return NotFound();
            }

            var flashcardSet = await _context.FlashcardSet
                .Include(f => f.Flashcards.Where(f => f.SetId == id && f.IsDeleted == false))
                .FirstAsync(m => m.SetId == id);
            // .FindAsync(id);
            if (flashcardSet == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", flashcardSet.UserId);
            return View(flashcardSet);
        }

        // POST: FlashcardSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid id, [Bind("SetId,Name,Description,Category,UserId,CreatedDate,UpdatedDate,IsDeleted")] FlashcardSet flashcardSet)
        {
            if (id != flashcardSet.SetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    flashcardSet.UpdatedDate = DateTime.Now;
                    _context.Update(flashcardSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlashcardSetExists(flashcardSet.SetId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", flashcardSet.UserId);
            return View(flashcardSet);
        }

        // POST: FlashcardSets/Edit/5
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateFlashcard(Guid setId, List<Flashcard> flashcards)
        {
            foreach (var flashcard in flashcards)
            {
                var flashcardId = flashcard.FlashcardId;
                flashcard.SetId = setId;
                flashcard.UpdatedDate = DateTime.Now;

                if (flashcardId != flashcard.FlashcardId)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            _context.UpdateRange(flashcards);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlashcardExists(flashcards[0].FlashcardId))
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

        // GET: FlashcardSets/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.FlashcardSet == null)
            {
                return NotFound();
            }

            var flashcardSet = await _context.FlashcardSet
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.SetId == id);
            if (flashcardSet == null)
            {
                return NotFound();
            }

            return View(flashcardSet);
        }

        // POST: FlashcardSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.FlashcardSet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FlashcardSet'  is null.");
            }
            var flashcardSet = await _context.FlashcardSet.FindAsync(id);
            if (flashcardSet != null)
            {
                // _context.FlashcardSet.Remove(flashcardSet);
                flashcardSet.IsDeleted = true;
            }

            // Also delete the report corresponding to the set. This is a hard delete because
            // Report doesn't have a IsDeleted property
            var report = await _context.Report.FirstOrDefaultAsync(r => r.SetId == flashcardSet.SetId);
            if (report != null)
            {
                _context.Report.Remove(report);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlashcardSetExists(Guid id)
        {
          return (_context.FlashcardSet?.Any(e => e.SetId == id)).GetValueOrDefault();
        }
        private bool FlashcardExists(Guid id)
        {
            return (_context.Flashcard?.Any(e => e.FlashcardId == id)).GetValueOrDefault();
        }
        public static List<FlashcardSet> GetNonDeletedSets(ApplicationDbContext _context)
        {
            return _context.FlashcardSet.Where(set => !set.IsDeleted).ToList();
        }
    }
}
