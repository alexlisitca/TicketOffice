using System;

namespace TicketOffice.Domain.Models
{
    public class GridFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ShowName { get; set; }
    }
}
