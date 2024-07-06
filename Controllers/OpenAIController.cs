using assignment4.Models;
using Microsoft.AspNetCore.Authorization; // Do not delete even if 'Using directive is unnecessary.'
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Chat;

namespace assignment4.Controllers
{
    // When testing this controller with Postman Desktop, you must comment out the Authorize statement.
    // During normal use of the web application, the Authorize statement should be un-commented.
    [Authorize(Roles = "Teacher, Student")]
    public class OpenAIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // OpenAI/AIReponse
        // Postman Desktop example:
        // POST v    https://localhost:XXXX/OpenAI/AIResponse
        // Key          Value
        // prompt       Give me more information about Programmer
        // * prompt MUST be prompt, value can be whatever.
        [HttpPost]
        public async Task<ContentResult> AIResponse(string prompt)
        {
            AIService myService = new();

            var response = await myService.API.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Temperature = 0.1,
                MaxTokens = 400, // Correlates to the maximum length of the response.
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.User, prompt)
                }
            });

            return Content(response.ToString());
        }
    }
}
