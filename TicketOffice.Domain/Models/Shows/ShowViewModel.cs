using System;
using System.Collections.Generic;
using System.Linq;
using TicketOffice.Domain.Entities;
using TicketOffice.Domain.Models.Tickets;

namespace TicketOffice.Domain.Models.Shows
{
    public class ShowViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan Duration { get; set; }
        public int TicketCount { get; set; }
        public ICollection<TicketViewModel> Tickets { get; set; }
        public ShowViewModel()
        {
            Tickets = new List<TicketViewModel>();
        }
        public ShowViewModel(ICollection<Ticket> tickets) : this()
        {
            Tickets = tickets.Select(t => new TicketViewModel() { Id = t.Id, UserId = t.UserId }).ToList();
        }
    }
}
