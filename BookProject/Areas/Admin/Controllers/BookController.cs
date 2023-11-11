using DataAccess.Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BookProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;  // DI cercevesi icin Program.cs'de servis kaydi yapilmasi gerekir
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
            else  // id dolu gelirse update islemi yapilacagi icin o id Book tablosunda bulunup dondurulsun
            {
                // update
                bookVM.Book = _unitOfWork.Book.Get(u => u.Id == id);
                return View(bookVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(BookVM bookVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)  // dosya bos degilse
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);  // benzersiz isim olusturur ve dosyanın uzantısını ekler
                    string bookPath = Path.Combine(wwwRootPath, @"images/book");

                    if (!string.IsNullOrEmpty(bookVM.Book.ImageUrl))  // sagda gosterilen image, yeni resim yuklendiginde eskisi silinip yenisi gelsin
                    {
                        // delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, bookVM.Book.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream=new FileStream(Path.Combine(bookPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    bookVM.Book.ImageUrl =  fileName;
                }
                if (bookVM.Book.Id == 0)
                {
                    _unitOfWork.Book.Add(bookVM.Book);
                }
                else
                {
                    _unitOfWork.Book.Update(bookVM.Book);
                }
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
