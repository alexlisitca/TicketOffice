using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketOffice.Domain.Services
{
    public interface INotificationService
    {
        Task NotifacteUserList(List<string> userList);
    }
}
