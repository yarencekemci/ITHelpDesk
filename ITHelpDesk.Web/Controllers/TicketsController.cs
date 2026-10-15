using ITHelpDesk.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace ITHelpDesk.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tickets = _context.Tickets.ToList();

            return View(tickets);
        }
        // yeni destek talebi oluşturma sayfasını açar.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}