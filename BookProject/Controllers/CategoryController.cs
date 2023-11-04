using BookProject.Data;
using BookProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> onjCategoryList=_db.Categories.ToList();
            return View(onjCategoryList);
        }
    }
}
