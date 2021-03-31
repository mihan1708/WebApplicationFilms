using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFilms.Models;
using WebApplicationFilms.ViewModels;

namespace WebApplicationFilms.Controllers
{
    public class HomeController : Controller
    {
        FilmDbContext db;
        public HomeController(FilmDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Films()
        {
            List<Film> films = db.Films.Include("Sessions").ToList();
            return View(films);
        }
        [HttpGet]
        public IActionResult Film(int id)
        {
            Film film = db.Films.Include(f=>f.Sessions).ThenInclude(f=>f.Hall)
                .FirstOrDefault(f => f.Id == id);
            ViewBag.NameFilm = film.Name;
            return View(film);
        }
        public IActionResult Sessions(int id)
        {
            List<Session> sessions = db.Sessions.Include(s => s.Hall)
                                                    .ThenInclude(h => h.Seats)
                                                .Include(s => s.Film)
                                                    .ThenInclude(f => f.Sessions)
                                                    .Where(s => s.Film.Id == id).ToList();
            return View(sessions);
        }
        public IActionResult Session(int id)
        {
            Session session = db.Sessions.Include(s=>s.Hall)
                                            .ThenInclude(h=>h.Seats)
                                         .Include(s=>s.Film)
                                            .ThenInclude(f=>f.Sessions)
                .FirstOrDefault(s => s.Id == id);
            List<Ticket> tickets = db.Tickets.Include(t=>t.Session)
                                                        .ThenInclude(s=>s.Hall)
                                             .Include(t => t.Session)
                                                        .ThenInclude(s => s.Film)
                                             .Include(t => t.Seat)
                                                        .ThenInclude(s => s.Hall)
                                             .Where(t => t.Session.Id == id).ToList();
            
            List<Seat> _purchaseSeats = new List<Seat>();
            if(tickets!=null)
            foreach (Ticket t in tickets)
            {
                _purchaseSeats.Add(t.Seat);
            }
            session.PurchasedSeats = _purchaseSeats;
            session.Hall.CountSeats = session.Hall.Seats.Count;
            session.Hall.CountRows = db.Seats.Where(s => s.Hall.Id == session.Hall.Id).Max(c => c.NumberRow);

            return View(session);
        }
      

    }
}
