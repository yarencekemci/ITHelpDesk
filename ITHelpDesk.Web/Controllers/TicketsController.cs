
using ITHelpDesk.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // Destek taleplerini listeler.
        public IActionResult Index()
        {
            var tickets = _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .ToList();

            return View(tickets);
        }

        // Destek talebinin detaylarını gösterir.
        public IActionResult Details(int id)
        {
            var ticket = _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // Düzenleme sayfasını açar.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // Düzenlenen destek talebini kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ITHelpDesk.Entities.Ticket ticket)
        {
            var existingTicket = _context.Tickets.Find(ticket.Id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            // Başlık ve açıklama doğrulaması.
            if (string.IsNullOrWhiteSpace(ticket.Title) ||
                ticket.Title.Length > 100 ||
                string.IsNullOrWhiteSpace(ticket.Description) ||
                ticket.Description.Length > 2000)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Başlık 1-100, açıklama ise 1-2000 karakter arasında olmalıdır.");

                return View(ticket);
            }

            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Destek talebini siler.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Yeni destek talebi oluşturma sayfasını açar.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ITHelpDesk.Entities.Ticket ticket)
        {
            // Title/Description doğrulaması Ticket.cs içindeki attribute'larla yapılır.
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

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
