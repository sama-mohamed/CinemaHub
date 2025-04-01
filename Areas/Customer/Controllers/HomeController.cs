using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories;
using CinemaWebSite.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CinemaWebSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly IMovieRepository movieRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly IActorRepository actorRepository;

        public HomeController(IMovieRepository movieRepository, ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository,IActorRepository actorRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
            this.actorRepository = actorRepository;
        }

        public IActionResult Index()
        {
            var movies = categoryRepository.Get(includes: [e => e.Movies]).ToList();
            ViewBag.Movies = movieRepository.Get(e=>e.MovieStatus==2, includes: [e => e.Category]).ToList();
            
            return View(movies);
        }

        public IActionResult Comedy()
        {
            var movies = categoryRepository.Get(includes: [e => e.Movies]).FirstOrDefault(e => e.Id == 2);
            return View(movies);


        }
        public IActionResult Drama()
        {
            var movies = categoryRepository.Get(includes: [e => e.Movies]).FirstOrDefault(e => e.Id == 3);
            return View(movies);


        }
        [HttpGet]
        public IActionResult ShowActors()
        {
            var actors = actorRepository.Get().ToList();
            return View(actors);

        }

        public IActionResult Details(int MovieId)
        {
            var movies = movieRepository.Get(includes: [e => e.Category, e => e.Cinema]).Include(e=>e.ActorMovies).ThenInclude(e=>e.Actor).FirstOrDefault(e=>e.Id== MovieId);
            return View(movies);
        }
        public IActionResult ShowCategory(int CategoryId) {
            var Movies = categoryRepository.Get(includes: [e => e.Movies]).FirstOrDefault(e => e.Id == CategoryId);
           return View(Movies);
            
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
