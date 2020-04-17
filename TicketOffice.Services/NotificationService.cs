using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketOffice.Domain.Services;

namespace TicketOffice.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger _logger;

        public NotificationService(ILogger<ShowService> logger)
        {
            _logger = logger;
        }

        public async Task NotifacteUserList(List<string> userIds)
        {
            foreach (var userId in userIds) 
            {
               _logger.LogInformation($"Notification send to user: {userId}");
            }
        }
    }
}
