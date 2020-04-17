using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketOffice.Domain.Entities;

namespace TicketOffice.Database
{
    public class TicketOfficeDbContext : IdentityDbContext
    {
        public TicketOfficeDbContext(DbContextOptions<TicketOfficeDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Show>().HasKey(s => s.Id);
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
            modelBuilder.Entity<Show>().HasMany(p => p.Tickets);
        }
    }
}
