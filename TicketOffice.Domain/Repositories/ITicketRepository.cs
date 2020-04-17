using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;

namespace TicketOffice.Domain.Repositories
{
    public interface ITicketRepository
    {
        Task<bool> BookTicket(Show show, Guid userId);
        Task<bool> UnbookTicket(Guid ticketId);
    }
}
