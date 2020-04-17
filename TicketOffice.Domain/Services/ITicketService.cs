using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models.Tickets;

namespace TicketOffice.Domain.Services
{
    public interface ITicketService
    {
        Task<TicketViewModel> BookTicket(Guid showId, Guid userId);
        Task<TicketViewModel> UnbookTicket(Guid ticketId);
    }
}
