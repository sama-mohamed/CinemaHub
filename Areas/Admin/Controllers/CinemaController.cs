using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories;
using CinemaWebSite.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CinemaWebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class CinemaController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;

        public CinemaController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }

        public IActionResult Index()
        {
           var Cenima= cinemaRepository.Get().ToList();
            return View(Cenima);
        }
        [HttpGet]
        public IActionResult AddCinema()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public IActionResult AddCinema(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                cinemaRepository.Create(cinema);
                cinemaRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        [HttpGet]
        public IActionResult EditCinema(int CinemaId)
        {
            var cenima = cinemaRepository.GetOne(e=>e.Id == CinemaId);
            if (ModelState.IsValid)
            {
                return View(cenima);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        [HttpPost]
        public IActionResult EditCinema(Cinema cinema)
        {
            if (ModelState.IsValid) {
                cinemaRepository.Edit(cinema);
                cinemaRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        public IActionResult DeleteCinema(int CinemaId)
        {
            var cinema = cinemaRepository.GetOne(e => e.Id == CinemaId);
            if (ModelState.IsValid) {
                cinemaRepository.Delete(cinema);
                cinemaRepository.Commit();
                return RedirectToAction("Index");
            
            }
            return RedirectToAction("NotFoundPage", "Home");


        }
        public IActionResult ShowMovies(int CinemaId) 
        {
            var Movies= cinemaRepository.Get(includes: [e=>e.Movies]).FirstOrDefault(e=>e.Id==CinemaId);
            return View(Movies);
        }




    }
}
