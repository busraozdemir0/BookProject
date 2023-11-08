using DataAccess.Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;

namespace BookProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;  // DI cercevesi icin Program.cs'de servis kaydi yapilmasi gerekir
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> onjCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(onjCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            //if (category.Name == category.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");  // name ve display order alani tamamen ayni olamaz hatasi
            //}
            //if (category.Name!=null && category.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "Test is an invalid value.");  // kullanici category name alanina test girerse test degerinin gecersiz oldugu uyarisi verilsin
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created succesfully";  // basarili mesaji dondurmek icin
                return RedirectToAction("Index");
            }

            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated succesfully";
                return RedirectToAction("Index");
            }

            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
