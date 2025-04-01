using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories;
using CinemaWebSite.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

		public IActionResult Index()
        {
          var categories= categoryRepository.Get().ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult EditCategory(int CategoryId) {
            var category = categoryRepository.GetOne(e=>e.Id==CategoryId);
            if (ModelState.IsValid)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        [HttpPost]
        public IActionResult EditCategory(Category category) {
            if (ModelState.IsValid)
            {
                categoryRepository.Edit(category);
                categoryRepository.Commit ();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult DeleteCategory(int CategoryId) {
            var category= categoryRepository.GetOne(e => e.Id == CategoryId);
            if (ModelState.IsValid) {
                categoryRepository.Delete(category);
                categoryRepository.Commit () ;
                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");

        }
    }
}
