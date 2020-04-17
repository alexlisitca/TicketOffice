using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;

namespace TicketOffice.Domain.Repositories
{
    public interface IShowRepository
    {
        Task<bool> Add(Show show);
        Task<Show> GetById(Guid showId);
        Task<bool> Update(Show show);
        Task<PagedList<Show>> GetList(GridFilter filter, int page, int pageSize);
    }
}
