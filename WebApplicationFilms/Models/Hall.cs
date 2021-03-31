using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Seat> Seats { get; set; }
        [NotMapped]
        public int CountSeats { get; set; }
        [NotMapped]
        public int CountRows { get; set; }
   
    }
}
