using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models;
using TicketOffice.Domain.Models.Shows;
using TicketOffice.Domain.Repositories;
using TicketOffice.Domain.Services;

namespace TicketOffice.Services
{
    public class ShowService: IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public ShowService(INotificationService notificationService, IShowRepository showRepository, ILogger<ShowService> logger)
        {
            _logger = logger;
            _showRepository = showRepository;
            _notificationService = notificationService;
        }

        public async Task<ShowViewModel> AddShow(string name, DateTime showDate, TimeSpan showDuration, int ticketCount)
        {

            var show = new Show()
            {
                Id = Guid.NewGuid(),
                Name = name,
                ShowDate = showDate,
                Duration = showDuration,
                TicketCount = ticketCount
            };

            try
            {
                return await _showRepository.Add(show);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<PagedList<ShowViewModel>> GetPage(GridFilter filter, int page, int pageSize)
        {
            try
            {
                return await _showRepository.GetList(filter, page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new PagedList<ShowViewModel>(Enumerable.Empty<ShowViewModel>(), 0, pageSize, 0);
            }
        }

        public async Task<Show> GetShow(Guid showId)
        {
            return await _showRepository.GetById(showId);
        }

        public async Task<ShowViewModel> UpdateShow(Guid showId, string name, DateTime showDate, TimeSpan showDuration, int ticketCount)
        {
            ShowViewModel result = null;
            try
            {
                var show = await _showRepository.GetById(showId);
                if (show == null)
                    throw new Exception($"ShowRepository.GetById return empty show by showId: {showId}");

                show.Duration = showDuration;
                show.Name = name;
                show.ShowDate = showDate;
                show.TicketCount = ticketCount;
                result = await _showRepository.Update(show);

                if (result != null)
                    await _notificationService.NotifacteUserList(show.Tickets.Select(x => x.UserId.ToString()).Distinct().ToList());
                else
                    throw new Exception($"ShowRepository.Update method return bad result for showId: {showId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }

            return result;
        }
    }
}
