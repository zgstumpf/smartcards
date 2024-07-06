using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace assignment4.Controllers
{
    public class AnswerModel
    {
        public Guid flashcardID { get; set; }
        public string selectedAnswer { get; set; }
    }
    [Authorize]
    public class PracticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string loggedInUserId;

        public PracticeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            loggedInUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        [HttpPost]
        public IActionResult CheckAnswer([FromBody] AnswerModel answer)
        {
            bool isCorrect = true;
            var flashcard = _context.Flashcard
                .FirstOrDefault(m => m.FlashcardId == answer.flashcardID);

            if(answer.selectedAnswer == flashcard.Front) {
                isCorrect = true;
                return Content("Correct");
            } else {
                isCorrect = false;
                return Content("Incorrect");
            }
        }

    }
}
