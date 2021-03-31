using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class FilmDbContext:DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Image> Images { get; set; }

        public FilmDbContext(DbContextOptions<FilmDbContext> options)
           : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
