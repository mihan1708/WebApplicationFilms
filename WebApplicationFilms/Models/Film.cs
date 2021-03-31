using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime Release { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Image> Images { get; set; }
        public string UrlTrailer { get; set; }
        public string Director { get; set; }
        public string GreatRoles { get; set; }
    }
}
