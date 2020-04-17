using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
using TicketOffice.Domain.Models.Shows;

namespace TicketOffice.Domain.Services
{
    public interface IShowService
    {
        Task<ShowViewModel> AddShow(string name, DateTime showDate, TimeSpan showDuration, int ticketCount);
        Task<ShowViewModel> UpdateShow(Guid showId, string name, DateTime showDate, TimeSpan showDuration, int ticketCount);
        Task<PagedList<ShowViewModel>> GetPage(GridFilter filter, int page, int pageSize = 10);
        Task<Show> GetShow(Guid showId);
    }
}
