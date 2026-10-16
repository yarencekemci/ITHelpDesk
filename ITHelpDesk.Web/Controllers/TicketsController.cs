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

        // Formdan gelen destek talebini veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ITHelpDesk.Entities.Ticket ticket)
        {
            // Kullanıcıdan alınmayan alanlara başlangıç değerleri veriyoruz.
            ticket.UserId = 1;
            ticket.CategoryId = 1;
            ticket.PriorityId = 1;
            ticket.StatusId = 1;
            ticket.CreatedDate = DateTime.Now;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}