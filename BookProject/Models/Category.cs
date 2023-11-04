using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookProject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]  // gosterilecek isim
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display Order must be between 1-100")]  // 1 ile 100 arasinda olmalidir
        public int DisplayOrder { get; set; } // goruntuleme sirasi
    }
}
