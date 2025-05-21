using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class CaptchaController() : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
