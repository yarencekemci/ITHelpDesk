
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

        // destek taleplerini listeler
        public IActionResult Index()
        {
            var tickets = _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .ToList();

            return View(tickets);
        }

        // destek talebinin detaylarını gösterir
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

        // düzenleme sayfasını açar
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Kategori ve öncelik seçeneklerini yükle
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Priorities = _context.Priorities.ToList();

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ITHelpDesk.Entities.Ticket ticket)
        {
            var existingTicket = _context.Tickets.Find(ticket.Id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            // başlık ve açıklama doğrulaması.
            if (string.IsNullOrWhiteSpace(ticket.Title) ||
                ticket.Title.Length > 100 ||
                string.IsNullOrWhiteSpace(ticket.Description) ||
                ticket.Description.Length > 2000)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Başlık 1-100, açıklama ise 1-2000 karakter arasında olmalıdır.");
            }

            // kategori ve öncelik doğrulaması
            if (!_context.Categories.Any(c => c.Id == ticket.CategoryId))
            {
                ModelState.AddModelError(nameof(ticket.CategoryId), "Geçerli bir kategori seçiniz.");
            }

            if (!_context.Priorities.Any(p => p.Id == ticket.PriorityId))
            {
                ModelState.AddModelError(nameof(ticket.PriorityId), "Geçerli bir öncelik seçiniz.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Priorities = _context.Priorities.ToList();

                return View(ticket);
            }

            // değişiklikleri mevcut kayda aktar
            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.CategoryId = ticket.CategoryId;
            existingTicket.PriorityId = ticket.PriorityId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // destek talebini siler
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

        // yeni destek talebi oluşturma sayfasını açar
        [HttpGet]
        public IActionResult Create()
        {
            // ktegorileri veritabanından getir.
            ViewBag.Categories = _context.Categories.ToList();

            // öncelikleri veritabanından getir.
            ViewBag.Priorities = _context.Priorities.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ITHelpDesk.Entities.Ticket ticket)
        {
            // formdan gelen kategori ve öncelik seçimlerini doğrula
            if (!_context.Categories.Any(c => c.Id == ticket.CategoryId))
            {
                ModelState.AddModelError(nameof(ticket.CategoryId), "Geçerli bir kategori seçiniz.");
            }

            if (!_context.Priorities.Any(p => p.Id == ticket.PriorityId))
            {
                ModelState.AddModelError(nameof(ticket.PriorityId), "Geçerli bir öncelik seçiniz.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Priorities = _context.Priorities.ToList();

                return View(ticket);
            }

            ticket.UserId = 1;
            ticket.StatusId = 1;
            ticket.CreatedDate = DateTime.Now;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
