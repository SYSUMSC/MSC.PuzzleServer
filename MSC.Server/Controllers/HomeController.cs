using Microsoft.AspNetCore.Mvc;

namespace MSC.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
