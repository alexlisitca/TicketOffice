using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketOffice.Database;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models.Tickets;
using TicketOffice.Domain.Repositories;

namespace TicketOffice.Infrastructure.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketOfficeDbContext _db;
        private readonly ILogger<TicketRepository> _logger;

        public TicketRepository(TicketOfficeDbContext db, ILogger<TicketRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<TicketViewModel> BookTicket(Show show, Guid userId)
        {
            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                Show = show,
                UserId = userId
            };

            try
            {
                await _db.Tickets.AddAsync(ticket);
                await _db.SaveChangesAsync();
                return new TicketViewModel() { Id = ticket.Id, UserId = ticket.UserId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<TicketViewModel> UnbookTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
                var record = _db.Tickets.Remove(ticket);
                await _db.SaveChangesAsync();
                return new TicketViewModel() { Id = ticket.Id, UserId = ticket.UserId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
