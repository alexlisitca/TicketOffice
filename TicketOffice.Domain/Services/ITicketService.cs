using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;

namespace TicketOffice.Domain.Services
{
    public interface ITicketService
    {
        Task<bool> BookTicket(Guid showId, Guid userId);
        Task<bool> UnbookTicket(Guid ticketId);
    }
}
