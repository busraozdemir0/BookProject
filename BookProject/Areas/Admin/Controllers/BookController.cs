using DataAccess.Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;

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
            List<Book> objBookList = _unitOfWork.Book.GetAll().ToList();
            return View(objBookList);
        }

        public IActionResult Upsert(int? id)  // create ve update bir arada yapacak metod
        {

            //ViewBag.CategoryList = CategoryList;  // viewbag kullanimi
            //ViewData["CategoryList"] = CategoryList;  // viewdata kullanimi

            BookVM bookVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Book = new Book()
            };
            if(id==null || id == 0)  // id bos veya sifir donerse create yapsin
            {
                return View(bookVM);
            }
            else  // id dolu gelirse update islemi yapsin
            {
                // update
                bookVM.Book = _unitOfWork.Book.Get(u => u.Id == id);
                return View(bookVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(BookVM bookVM, IFormFile? file)
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
                _unitOfWork.Book.Add(bookVM.Book);
                _unitOfWork.Save();
                TempData["success"] = "Book created succesfully";  // basarili mesaji dondurmek icin
                return RedirectToAction("Index");
            }
            else
            {
                bookVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(bookVM);
            }

        }
        // - Edit islemi de Upsert metodu icerisinde kontrollerle gerceklestiriliyor
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Book? bookFromDb = _unitOfWork.Book.Get(u => u.Id == id);

        //    if (bookFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(bookFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Book.Update(book);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Book updated succesfully";
        //        return RedirectToAction("Index");
        //    }

        //    return View();

        //}
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
