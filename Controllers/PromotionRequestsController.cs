using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace assignment4.Controllers
{
    public class PromotionRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string loggedInUserId;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PromotionRequestsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            loggedInUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _userManager = userManager;
            _roleManager = roleManager;

        }

        // GET: PromotionRequests
        [Authorize (Roles ="Teacher")]
        public async Task<IActionResult> Index()
        {
              return _context.PromotionRequest != null ? 
                          View(await _context.PromotionRequest.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PromotionRequest'  is null.");
        }

        // GET
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UndecidedRequests()
        {
            var promotionRequests = await _context.PromotionRequest
                .Where(pr => pr.WasPromotionGranted == null)
                .OrderBy(pr => pr.RequestedDate)
                .ToListAsync();

            if (promotionRequests == null)
            {
                return Problem("Unable to retrieve PromotionRequests for the current user.");
            }

            return View(promotionRequests);
        }

        // GET: PromotionRequests/StudentView
        [Authorize (Roles = "Student")]
        public async Task<IActionResult> StudentView()
        {
            var promotionRequests = await _context.PromotionRequest
                .Where(pr => pr.RequesterUserId == new Guid(loggedInUserId))
                .OrderByDescending(pr => pr.RequestedDate)
                .ToListAsync();

            if (promotionRequests == null)
            {
                return Problem("Unable to retrieve PromotionRequests for the current user.");
            }

            return View(promotionRequests);
        }

        // GET: PromotionRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PromotionRequest == null)
            {
                return NotFound();
            }

            var promotionRequest = await _context.PromotionRequest
                .FirstOrDefaultAsync(m => m.PromotionRequestId == id);
            if (promotionRequest == null)
            {
                return NotFound();
            }

            return View(promotionRequest);
        }

        // GET: PromotionRequests/Create
        [Authorize(Roles = "Student")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PromotionRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([Bind("PromotionRequestId,RequesterUserId,RequesterUserName,RequestedDate,RequestDetails,WasPromotionGranted,GrantedDate,GrantedById,GrantedByName")] PromotionRequest promotionRequest)
        {
            if (ModelState.IsValid)
            {
                promotionRequest.PromotionRequestId = Guid.NewGuid();

                // Since only students can access PromotionRequests/Create, loggedInUserId and User.Identity.Name
                // will be for the student.
                promotionRequest.RequesterUserId = new Guid(loggedInUserId);
                promotionRequest.RequesterUserName = User.Identity.Name;

                promotionRequest.RequestedDate = DateTime.Now;

                _context.Add(promotionRequest);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("StudentView");
            }
            return View(promotionRequest);
        }

        // GET: PromotionRequests/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PromotionRequest == null)
            {
                return NotFound();
            }

            var promotionRequest = await _context.PromotionRequest.FindAsync(id);
            if (promotionRequest == null)
            {
                return NotFound();
            }

            // Teachers cannot edit a PromotionRequest to alter the value of WasPromotionGranted if WasPromotionGranted
            // has already been accepted (set to true) or rejected (set to false)
            if (promotionRequest.WasPromotionGranted != null)
            {
                return NotFound();
            }

            return View(promotionRequest);
        }

        // POST: PromotionRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid id, [Bind("PromotionRequestId,RequesterUserId,RequesterUserName,RequestedDate,RequestDetails,WasPromotionGranted,GrantedDate,GrantedById,GrantedByName")] PromotionRequest promotionRequest)
        {
            if (id != promotionRequest.PromotionRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Even if the request was rejected, use these fields to show who rejected the request.
                    // Since only teachers can access PromotionRequests/Edit, loggedInUserId and User.Identity.Name
                    // will be the teacher's.
                    promotionRequest.GrantedDate = DateTime.Now;
                    promotionRequest.GrantedById = new Guid(loggedInUserId);
                    promotionRequest.GrantedByName = User.Identity.Name;

                    _context.Update(promotionRequest);
                    await _context.SaveChangesAsync();

                    if(promotionRequest.WasPromotionGranted == true)
                    {
                        // Find the student with a UserId corresponding to RequesterUserId, remove them
                        // from Student Role, and add them to Teacher role.
                        var olduser = await _userManager.FindByIdAsync(promotionRequest.RequesterUserId.ToString());
                        await _userManager.RemoveFromRoleAsync(olduser, "Student");
                        await _userManager.AddToRoleAsync(olduser, "Teacher");


                        _context.Update(promotionRequest);
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionRequestExists(promotionRequest.PromotionRequestId))
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
            return View(promotionRequest);
        }

        // GET: PromotionRequests/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PromotionRequest == null)
            {
                return NotFound();
            }

            var promotionRequest = await _context.PromotionRequest
                .FirstOrDefaultAsync(m => m.PromotionRequestId == id);
            if (promotionRequest == null)
            {
                return NotFound();
            }

            return View(promotionRequest);
        }

        // POST: PromotionRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PromotionRequest == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PromotionRequest'  is null.");
            }
            var promotionRequest = await _context.PromotionRequest.FindAsync(id);
            if (promotionRequest != null)
            {
                _context.PromotionRequest.Remove(promotionRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionRequestExists(Guid id)
        {
          return (_context.PromotionRequest?.Any(e => e.PromotionRequestId == id)).GetValueOrDefault();
        }
    }
}
