namespace Cars.PL.Controllers
{
    public class ChatController : Controller
    {
        private readonly IOllamaService _ollamaService;

        public ChatController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                ViewBag.Response = "Please enter a question";
                return View();
            }

            ViewBag.LastPrompt = prompt;

            
            var response = await _ollamaService.AskModelAsync(prompt,true);

            ViewBag.Response = response;
            return View();
        }

    }
}