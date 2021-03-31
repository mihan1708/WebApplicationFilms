using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFilms.Models;
using WebApplicationFilms.ViewModels;

namespace WebApplicationFilms.Controllers
{
    public class TicketsController : Controller
    {
        FilmDbContext db;
        UserManager<User> _userManager;
        public TicketsController(FilmDbContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Tickets()
        {
            List<Ticket> tickets;
            if (HttpContext.User.IsInRole("User"))
            {
                tickets = db.Tickets.Include(t => t.Session).ThenInclude(s => s.Hall)
                                             .Include(t => t.Session).ThenInclude(s => s.Film)
                                             .Include(t => t.Seat).Where(l=>l.UserEmail == HttpContext.User.Identity.Name).ToList();
                return View(tickets);
            }
            else if(HttpContext.User.IsInRole("Admin"))
            {
                tickets = db.Tickets.Include(t => t.Session).ThenInclude(s => s.Hall)
                                             .Include(t => t.Session).ThenInclude(s => s.Film)
                                             .Include(t => t.Seat).ToList();
                return View(tickets);
            }
            return null;
            
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult TicketsForUser(int id)
        {
            List<Ticket> tickets = db.Tickets.Include(t => t.Session).ThenInclude(s => s.Hall)
                                             .Include(t => t.Session).ThenInclude(s => s.Film)
                                             .Include(t => t.Seat)./*Where(tickets=>tickets.UserId == id)*/ToList();
            return View(tickets);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Buy(int SessionId, int[] SeatId)
        {
            if (SessionId > 0 && SeatId.Length > 0)
            {

                CreateTicketViewModel _newTicker = new CreateTicketViewModel { SeatId = SeatId, SessionId = SessionId, Date = DateTime.Now };
                return View(_newTicker);
                //return View();

            }
            return View("Что-то пошло не так");

        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public string Buy(CreateTicketViewModel createTicketViewModel)
        {
            for(int i=0;i<createTicketViewModel.SeatId.Length;i++)
            {
                Ticket t = new Ticket
                {
                    DateTime = createTicketViewModel.Date,
                    Seat = db.Seats.Find(createTicketViewModel.SeatId[i]),
                    Session = db.Sessions.Find(createTicketViewModel.SessionId),
                    UserEmail = HttpContext.User.Identity.Name
                };
                
                if (t.Session != null && t.Seat != null)
                {
                    db.Tickets.Add(t);
                }
                else
                    return "Что-то пошло не так";

            }
            db.SaveChanges();
            return "Спасибо за покупку";


        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            return View(ticket);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Ticket t)
        {
            Ticket ticket = db.Tickets.Find(t.Id);
            if (ticket != null)
            {
                db.Tickets.Remove(ticket);
                db.SaveChanges();
            }
            return RedirectToAction("Tickets");
        }
    }
}
