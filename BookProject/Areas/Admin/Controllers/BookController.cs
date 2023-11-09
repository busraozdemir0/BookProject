using DataAccess.Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;

namespace BookProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;  // DI cercevesi icin Program.cs'de servis kaydi yapilmasi gerekir
        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Book> onjBookList = _unitOfWork.Book.GetAll().ToList();
            return View(onjBookList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
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
                _unitOfWork.Book.Add(book);
                _unitOfWork.Save();
                TempData["success"] = "Book created succesfully";  // basarili mesaji dondurmek icin
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
            Book? bookFromDb = _unitOfWork.Book.Get(u => u.Id == id);

            if (bookFromDb == null)
            {
                return NotFound();
            }
            return View(bookFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Book.Update(book);
                _unitOfWork.Save();
                TempData["success"] = "Book updated succesfully";
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
            Book? bookFromDb = _unitOfWork.Book.Get(u => u.Id == id);

            if (bookFromDb == null)
            {
                return NotFound();
            }
            return View(bookFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Book obj = _unitOfWork.Book.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Book.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Book deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
