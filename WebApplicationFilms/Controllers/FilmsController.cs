using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFilms.Models;
using WebApplicationFilms.ViewModels;

namespace WebApplicationFilms.Controllers
{
    
    public class FilmsController : Controller
    {
        FilmDbContext db;
        IWebHostEnvironment _appEnvironment;
        public FilmsController(FilmDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index() => View(db.Films.ToList());
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int filmId)
        {
            if(filmId > 0)
            {
                Film film = db.Films.Include(f=>f.Sessions.Where(s => s.Date >= DateTime.Now).OrderBy(s => s.Date)).ThenInclude(f => f.Hall)
                                   .Include(f => f.Images)
               .FirstOrDefault(f => f.Id == filmId);
                if (film != null)
                    return View(film);
                else return Redirect("/films");
            }
            return Redirect("/films");

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Film film)
        {
            if(film!=null)
            {
                Film _film = db.Films.Include(f=>f.Images).Include(s=>s.Sessions).ThenInclude(h=>h.Hall).FirstOrDefault(f=>f.Id == film.Id);
                _film.Name = film.Name;
                _film.Genre = film.Genre;
                _film.Description = film.Description;
                _film.Duration = film.Duration;
                _film.Release = film.Release;
                _film.GreatRoles = film.GreatRoles;
                _film.UrlTrailer = film.UrlTrailer;
                _film.Director = film.Director;
                db.SaveChanges();
                return View(_film);
            }
            return Redirect("/films");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int filmId)
        {
            if(filmId>0)
            {
                Film _film = db.Films.FirstOrDefault(film => film.Id == filmId);
                if (_film != null)
                    return View(_film);
                else return Redirect("/films");
            }
            return Redirect("/films");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Film film)
        {
            if (film != null)
            {
                Film _film = db.Films.Find(film.Id);
                if (_film != null)
                {
                    db.Films.Remove(_film);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return Redirect("/films");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Film film)
        {
            if(film!=null)
            {
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Redirect("/films");
        }
        [HttpGet]
        public IActionResult Film(int filmId)
        {
            if (filmId > 0)
            {
                Film _film = db.Films.Include(f => f.Sessions.Where(s=>s.Date>=DateTime.Now).OrderBy(s=>s.Date)).ThenInclude(f => f.Hall)
                                    .Include(f=>f.Images)
                .FirstOrDefault(f => f.Id == filmId);
                if(_film!=null)
                {
                    ViewBag.NameFilm = _film.Name;
                    return View(_film);
                }
            }
            return Redirect("/films");

        }
        public IActionResult Films()
        {
            List<Film> _films = db.Films.Include("Sessions").Include(f=>f.Images).ToList();
            return View("Index",_films);
            
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateSession(int FilmId)
        {
            Film film = db.Films.Find(FilmId);
            CreateSessionViewModel createSession = new CreateSessionViewModel
            {
                FilmName = film.Name,
                Halls = db.Halls.ToList()
            };
            
            return View(createSession);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateSession(CreateSessionViewModel sessionViewModel)
        {
            if(sessionViewModel!=null)
            {
                Session newSession = new Session
                {
                    Date = sessionViewModel.Date,
                    Film = db.Films.FirstOrDefault(f => f.Name == sessionViewModel.FilmName),
                    Hall = db.Halls.FirstOrDefault(h => h.Name == sessionViewModel.HallName),
                    Price = sessionViewModel.Price

                };
                db.Sessions.Add(newSession);
                db.SaveChanges();
                return RedirectToAction("Film",newSession.Film.Id);
            }
            return RedirectToAction("Film");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Sessions(int filmId)
        {
            List<Session> _sessions = db.Sessions.Include(s => s.Hall)
                                                    .ThenInclude(h => h.Seats)
                                                .Include(s => s.Film)
                                                    .ThenInclude(f => f.Sessions)
                                                    .Where(s => s.Film.Id == filmId).ToList();
            //_sessions = _sessions.Where(s => s.Date >= DateTime.Now).ToList();
            return View(_sessions);
        }
        public IActionResult Session(int sessionId)
        {
            if (sessionId > 0)
            {
                Session _session = db.Sessions.Include(s => s.Hall)
                                            .ThenInclude(h => h.Seats)
                                         .Include(s => s.Film)
                                            .ThenInclude(f => f.Sessions)
                .FirstOrDefault(s => s.Id == sessionId);
                List<Ticket> _tickets = db.Tickets.Include(t => t.Session)
                                                            .ThenInclude(s => s.Hall)
                                                 .Include(t => t.Session)
                                                            .ThenInclude(s => s.Film)
                                                 .Include(t => t.Seat)
                                                            .ThenInclude(s => s.Hall)
                                                 .Where(t => t.Session.Id == sessionId).ToList();

                List<Seat> _purchaseSeats = new List<Seat>();
                if (_tickets != null)
                    foreach (Ticket t in _tickets)
                    {
                        _purchaseSeats.Add(t.Seat);
                    }
                _session.PurchasedSeats = _purchaseSeats;
                _session.Hall.CountSeats = _session.Hall.Seats.Count;
                _session.Hall.CountRows = db.Seats.Where(s => s.Hall.Id == _session.Hall.Id).Max(c => c.NumberRow);
                return View(_session);
            }
            
            return View("Films");


        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddFile(int filmId)
        {
            Film _film = db.Films.Include(f=>f.Images).FirstOrDefault(f => f.Id == filmId);
            return View(_film);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, int filmId)//айди фильма
        {
            Film film = db.Films.Include(f => f.Sessions).ThenInclude(f => f.Hall)
                                    .Include(f => f.Images)
                .FirstOrDefault(f => f.Id == filmId);
            if (film != null)
                if (uploadedFile != null)
                {
                    // путь к папке Images
                    string path = "/Images/" + uploadedFile.FileName;
                    // сохраняем файл в папку Images в каталоге wwwroot
                    string fullPath = _appEnvironment.WebRootPath + "/Images/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                        Image _image = new Image { Name = uploadedFile.FileName, Film = db.Films.FirstOrDefault(f => f.Id == filmId) };
                        db.Images.Add(_image);
                        db.SaveChanges();
                    }
                }
            return View("Edit", film);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RemoveFileFromServer(int imageId)//айди картинки
        {
            Image _image = db.Images.Include(i=>i.Film).FirstOrDefault(i=>i.Id==imageId);
            string fullPath = _appEnvironment.WebRootPath+"/Images/" + _image.Name;
            if (!System.IO.File.Exists(fullPath))
                return RedirectToAction("Edit", _image.Film.Id);
            else//если найден в папке такой же файл
                try
                {
                    if(db.Images.Where(i=>i.Name == _image.Name).Count() > 1)
                    {
                        
                        db.Images.Remove(_image);
                        db.SaveChanges();
                        
                    }
                    else if(db.Images.Where(i => i.Name == _image.Name).Count() == 1)
                    {
                        System.IO.File.Delete(fullPath);
                        db.Images.Remove(_image);
                        db.SaveChanges();
                        
                        
                    }
                    
                }
                catch (Exception e)
                {
                    //Debug.WriteLine(e.Message);
                }
            Film film = db.Films.Include(f => f.Sessions).ThenInclude(f => f.Hall)
                                           .Include(f => f.Images)
                       .FirstOrDefault(f => f.Id == _image.Film.Id);
            return View("Edit", film);
        }
        
    }
}
