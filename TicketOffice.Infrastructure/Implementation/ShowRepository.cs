using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketOffice.Database;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
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

        public async Task<bool> Add(Show show)
        {
            try
            {
                var result = await _db.Shows.AddAsync(show);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<Show> GetById(Guid showId)
        {
            return await _db.Shows.Include(s => s.Tickets).Where(s => s.Id == showId).FirstOrDefaultAsync();
        }

        public async Task<PagedList<Show>> GetList(GridFilter filter, int page, int pageSize)
        {
            var shows = _db.Shows.AsQueryable();

            if (filter.FromDate != null)
                shows = shows.Where(s => s.ShowDate > filter.FromDate);

            if (filter.ToDate != null)
                shows = shows.Where(s => s.ShowDate < filter.ToDate);
            
            if (!string.IsNullOrEmpty(filter.ShowName))
                shows = shows.Where(s => s.Name.Contains(filter.ShowName));

            shows = shows.Skip(page * pageSize).Take(pageSize).AsQueryable();
            return new PagedList<Show>(shows, page, pageSize, _db.Shows.Count());
        }

        public async Task<bool> Update(Show show)
        {
            try
            {
                var result = _db.Shows.Update(show);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
