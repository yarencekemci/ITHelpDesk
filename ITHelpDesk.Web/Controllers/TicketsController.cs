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

        public IActionResult Index()
        {
            var tickets = _context.Tickets
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .ToList();

            return View(tickets);
        }

        // destek talebinin detaylarını gösterir.
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

        // destek talebini düzenleme sayfasını açar.
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

        // düzenlenen destek talebini veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ITHelpDesk.Entities.Ticket ticket)
        {
            var existingTicket = _context.Tickets.Find(ticket.Id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // destek talebini siler.
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

        // yeni destek talebi oluşturma sayfasını açar.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // formdan gelen destek talebini veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ITHelpDesk.Entities.Ticket ticket)
        {
            // Model doğrulamasından geçmeyen verileri kaydetme.
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