using Microsoft.AspNetCore.Mvc;

namespace ITHelpDesk.Web.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
