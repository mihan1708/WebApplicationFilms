using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFilms.Models;

namespace WebApplicationFilms.ViewModels
{
    public class CreateSessionViewModel
    {
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string HallName { get; set; }
        public string FilmName { get; set; }
        public List<Hall> Halls { get; set; }
    }
}
