using BookProjectRazor_Temp.Data;
using BookProjectRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookProjectRazor_Temp.Pages.Categories
{
    [BindProperties]  // Edit sayfasinin on yuzunde Category.Id yazarak erisebilmek icin
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)  // mvc'deki httpget yapisi gibi calisir
        {
            if(id != null && id != 0)
            {
                Category = _db.Categories.Find(id);
            }
        }
        public IActionResult OnPost() // mvc'deki httppost yapisi gibi calisir
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["success"] = "Category updated succesfully";
                return RedirectToPage("Index");
            }

            return Page();

        }
    }
}
