using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name is Mandatory")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [DisplayName("Category Name")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Display Order")]

        [Range(1,100, ErrorMessage = "The Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
