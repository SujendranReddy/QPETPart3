using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
