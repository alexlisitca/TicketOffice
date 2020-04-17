using System;

namespace TicketOffice.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Show Show { get; set; }
    }
}
