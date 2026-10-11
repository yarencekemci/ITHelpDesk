using ITHelpDesk.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}