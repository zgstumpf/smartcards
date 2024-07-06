using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace assignment4.Controllers
{
    public class FlashcardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string loggedInUserId;

        public FlashcardsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            loggedInUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        // GET: Flashcards
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Flashcard
                .Include(f => f.FlashcardSet)
                .Include(f => f.User)
                .Where(f => f.IsDeleted == false);
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: Flashcards/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Flashcard == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcard
                .Include(f => f.FlashcardSet)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FlashcardId == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // GET: Flashcards/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            //ViewData["SetId"] = new SelectList(_context.FlashcardSet, "SetId", "Name");
            ViewData["SetId"] = new SelectList(FlashcardSetsController.GetNonDeletedSets(_context), "SetId", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["User"] = new SelectList(_context.Users, "UserName", "UserName");
            return View();
        }

        // POST: Flashcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("FlashcardId,Front,Back,SetId,UserId,CreatedDate,UpdatedDate,IsDeleted")] Flashcard flashcard)
        {
            if (ModelState.IsValid)
            {
                flashcard.FlashcardId = Guid.NewGuid();
                flashcard.UserId = loggedInUserId;
                flashcard.CreatedDate = DateTime.Now;
                flashcard.UpdatedDate = DateTime.Now;
                flashcard.IsDeleted = false;
               
                _context.Add(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SetId"] = new SelectList(_context.FlashcardSet, "SetId", "Name", flashcard.SetId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", flashcard.UserId);
            
            return View(flashcard);
        }

        // GET: Flashcards/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Flashcard == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcard.FindAsync(id);
            if (flashcard == null)
            {
                return NotFound();
            }
            ViewData["SetId"] = new SelectList(FlashcardSetsController.GetNonDeletedSets(_context), "SetId", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["User"] = new SelectList(_context.Users, "UserName", "UserName");

            return View(flashcard);
        }

        // POST: Flashcards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid id, [Bind("FlashcardId,Front,Back,SetId,UserId,CreatedDate,UpdatedDate,IsDeleted")] Flashcard flashcard)
        {
            if (id != flashcard.FlashcardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    flashcard.UpdatedDate = DateTime.Now;
                    _context.Update(flashcard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlashcardExists(flashcard.FlashcardId))
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
            ViewData["SetId"] = new SelectList(_context.FlashcardSet, "SetId", "Name", flashcard.SetId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", flashcard.UserId);
            return View(flashcard);
        }

        // GET: Flashcards/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Flashcard == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcard
                .Include(f => f.FlashcardSet)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FlashcardId == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // POST: Flashcards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Flashcard == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Flashcard'  is null.");
            }
            var flashcard = await _context.Flashcard.FindAsync(id);
            if (flashcard != null)
            {
                // _context.Flashcard.Remove(flashcard);
                flashcard.IsDeleted = true;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlashcardExists(Guid id)
        {
          return (_context.Flashcard?.Any(e => e.FlashcardId == id)).GetValueOrDefault();
        }
    }
}
