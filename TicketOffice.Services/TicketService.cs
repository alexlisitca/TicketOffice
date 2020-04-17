using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketOffice.Domain.Repositories;
using TicketOffice.Domain.Services;

namespace TicketOffice.Services
{
    public class TicketService : ITicketService
    {
        private readonly IShowRepository _showRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger _logger;

        public TicketService(ITicketRepository ticketRepository, IShowRepository showRepository, ILogger<ShowService> logger)
        {
            _logger = logger;
            _ticketRepository = ticketRepository;
            _showRepository = showRepository;
        }

        public async Task<bool> BookTicket(Guid showId, Guid userId)
        {
            try
            {
                var show = await _showRepository.GetById(showId);

                if (show.Tickets.Count >= show.TicketCount)
                    return false;

                return await _ticketRepository.BookTicket(show, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> UnbookTicket(Guid ticketId)
        {
            try
            {
                return await _ticketRepository.UnbookTicket(ticketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
