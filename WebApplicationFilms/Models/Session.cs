using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Hall Hall { get; set; }
        public Film Film { get; set; }
        public int Price { get; set; }
        //купленные места
        [NotMapped]
        public List<Seat> PurchasedSeats { get; set; }

    }
}
