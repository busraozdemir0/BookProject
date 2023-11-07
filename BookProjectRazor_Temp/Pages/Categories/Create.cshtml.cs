using BookProjectRazor_Temp.Data;
using BookProjectRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookProjectRazor_Temp.Pages.Categories
{
    [BindProperties]   // model ustunde tanimlanacaksa
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        //[BindProperty]  // attribute’u ile iþaretlediðimizde bu özelliðimizi ön yüzde kullanabilir hale getiriyoruz.
        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            
        }
        public IActionResult OnPost()
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
    }
}
