using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class CaptchaController() : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
