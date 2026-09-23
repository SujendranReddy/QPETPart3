using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string SubCategoryName { get; set; } = string.Empty;

        public Category Category { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
