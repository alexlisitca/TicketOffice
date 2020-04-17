using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;

namespace TicketOffice.Domain.Services
{
    public interface IShowService
    {
        Task<bool> AddShow(string name, DateTime showDate, TimeSpan showDuration, int ticketCount);
        Task<bool> UpdateShow(Guid showId, string name, DateTime showDate, TimeSpan showDuration, int ticketCount);
        Task<PagedList<Show>> GetPage(GridFilter filter, int page, int pageSize = 10);
        Task<Show> GetShow(Guid showId);
    }
}
