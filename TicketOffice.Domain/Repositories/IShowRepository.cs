using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
using TicketOffice.Domain.Models.Shows;

namespace TicketOffice.Domain.Repositories
{
    public interface IShowRepository
    {
        Task<ShowViewModel> Add(Show show);
        Task<Show> GetById(Guid showId);
        Task<ShowViewModel> Update(Show show);
        Task<PagedList<ShowViewModel>> GetList(GridFilter filter, int page, int pageSize);
    }
}
