namespace Cars.PL.Controllers
{
    public class GeminiController : Controller
    {
        private readonly IGeminiService _geminiService;

        public GeminiController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<JsonResult> SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return Json(new { success = false, response = "Message cannot be empty." });
            }

            var response = await _geminiService.SendMessageAsync(message);
            return Json(new { success = true, response = response });
        }
        public async Task<IActionResult> ListModels()
        {
            var models = await _geminiService.GetAvailableModelsAsync();
            return View(models);
        }
    }
}
