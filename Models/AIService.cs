using OpenAI_API;

namespace assignment4.Models
{
    public class AIService
    {
        public OpenAIAPI API { get; set; }
        public AIService() {
            string api_key = ""; // TODO
            API = new OpenAIAPI(api_key);
        }
    }
}
