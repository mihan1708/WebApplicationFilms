using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.ViewModels
{
    public class CreateTicketViewModel
    {
        public DateTime Date { get; set; }
        public int SessionId { get; set; }
        public int[] SeatId { get; set; }
    }
}
