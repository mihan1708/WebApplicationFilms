using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int NumberRow { get; set; }
        public int Number { get; set; }
        public Hall Hall { get; set; }
        
        //public enum State {  free, busy }
        //[NotMapped]
        //public State state { get; set; }
    }
}
