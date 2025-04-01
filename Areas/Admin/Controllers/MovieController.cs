using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories;
using CinemaWebSite.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICinemaRepository CinemaRepository;
        private readonly ICategoryRepository categoryRepository;

        public MovieController(IMovieRepository movieRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository)
        {
            this.movieRepository = movieRepository;
            CinemaRepository = cinemaRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var movies = movieRepository.Get(includes: [e=>e.Category,e=>e.Cinema]).ToList();
            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = categoryRepository.Get().ToList();
            ViewBag.Cinemas = CinemaRepository.Get().ToList();
            return View(new Movie());
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile file)
        {
            ModelState.Remove("ImgUrl");
            ModelState.Remove("file");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    using (var Stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(Stream);

                    }
                    movie.ImgUrl = fileName;
                }



                movieRepository.Create(movie);
                movieRepository.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = categoryRepository.Get().ToList();
            ViewBag.Cinemas = CinemaRepository.Get().ToList();
            return View(movie);
        }

        [HttpGet]
        public IActionResult Edit(int MovieId)
        {
            var movie = movieRepository.Get(includes: [e => e.Category, e => e.Cinema]).FirstOrDefault(e => e.Id == MovieId);
            ViewBag.Categories = categoryRepository.Get().ToList();
            ViewBag.Cinemas = CinemaRepository.Get().ToList();
            return View(movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile file)
        {
            ModelState.Remove("ImgUrl");
            ModelState.Remove("file");


            if (ModelState.IsValid)
            {
                var MovieInDb = movieRepository.GetOne(e=>e.Id==movie.Id, tracked:false);
                if (MovieInDb == null)
                {
                    return NotFound();
                }

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", MovieInDb.ImgUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    using (var Stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(Stream);

                    }
                    movie.ImgUrl = fileName;
                }
                else
                {
                    movie.ImgUrl = MovieInDb.ImgUrl;
                }



                movieRepository.Edit(movie);
                movieRepository.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = categoryRepository.Get().ToList();
            ViewBag.Cinemas = CinemaRepository.Get().ToList();
            return View(movie);

        }



        public IActionResult Delete(int MovieId)
        {

            var movie = movieRepository.GetOne(e=>e.Id==MovieId);
            if (movie != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
               movieRepository.Delete(movie);
               movieRepository.Commit() ;
                return RedirectToAction("Index");

            }
            return RedirectToAction("NotFoundPage", "Home");
        }
    }

}