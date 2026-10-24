
using ITHelpDesk.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk.DataAccess
{
    // Identity kullanıcı ve rol tablolarını da yönetir
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // user silindiğinde ilişkili Comment kayıtları otomatik silinmesin
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
