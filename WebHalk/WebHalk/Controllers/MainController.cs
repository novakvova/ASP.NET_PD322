using Microsoft.AspNetCore.Mvc;

namespace WebHalk.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
