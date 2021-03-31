using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        //public int SessionId { get; set; }
        public Session Session { get; set; }
        public DateTime DateTime { get; set; }
        //public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public string UserEmail { get; set; }
    }
}
