using System;
using System.Collections.Generic;

namespace TicketOffice.Domain.Entities
{
    public class Show
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan Duration { get; set; }
        public int TicketCount { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public Show()
        {
            Tickets = new List<Ticket>();
        }
    }
}
