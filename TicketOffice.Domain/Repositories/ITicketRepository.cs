using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models.Tickets;

namespace TicketOffice.Domain.Repositories
{
    public interface ITicketRepository
    {
        Task<TicketViewModel> BookTicket(Show show, Guid userId);
        Task<TicketViewModel> UnbookTicket(Guid ticketId);
    }
}
