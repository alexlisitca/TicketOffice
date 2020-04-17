using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketOffice.Database;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
using TicketOffice.Domain.Models.Shows;
using TicketOffice.Domain.Models.Tickets;
using TicketOffice.Domain.Repositories;

namespace TicketOffice.Infrastructure.Implementation
{
    public class ShowRepository : IShowRepository
    {
        private readonly TicketOfficeDbContext _db;
        private readonly ILogger<ShowRepository> _logger;

        public ShowRepository(TicketOfficeDbContext db, ILogger<ShowRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ShowViewModel> Add(Show show)
        {
            try
            {
                var result = await _db.Shows.AddAsync(show);
                await _db.SaveChangesAsync();
                return new ShowViewModel(show.Tickets) { Id = show.Id, Name = show.Name, Duration = show.Duration, ShowDate = show.ShowDate, TicketCount = show.TicketCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Show> GetById(Guid showId)
        {
            return await _db.Shows.Include(s => s.Tickets).Where(s => s.Id == showId).FirstOrDefaultAsync();
        }

        public async Task<PagedList<ShowViewModel>> GetList(GridFilter filter, int page, int pageSize)
        {
            var shows = _db.Shows.Include(s => s.Tickets).AsQueryable();

            if (filter.FromDate != null)
                shows = shows.Where(s => s.ShowDate > filter.FromDate);

            if (filter.ToDate != null)
                shows = shows.Where(s => s.ShowDate < filter.ToDate);
            
            if (!string.IsNullOrEmpty(filter.ShowName))
                shows = shows.Where(s => s.Name.Contains(filter.ShowName));

            shows = shows.Skip(page * pageSize).Take(pageSize).AsQueryable();

            var showVm = shows.Select(
                s => new ShowViewModel(s.Tickets) {
                    Id = s.Id, 
                    Name = s.Name, 
                    Duration = s.Duration, 
                    ShowDate = s.ShowDate, 
                    TicketCount = s.TicketCount 
                }
            );

            return new PagedList<ShowViewModel>(showVm, page, pageSize, _db.Shows.Count());
        }

        public async Task<ShowViewModel> Update(Show show)
        {
            try
            {
                var result = _db.Shows.Update(show);
                await _db.SaveChangesAsync();
                return new ShowViewModel(show.Tickets) { Id = show.Id, Name = show.Name, Duration = show.Duration, ShowDate = show.ShowDate, TicketCount = show.TicketCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
