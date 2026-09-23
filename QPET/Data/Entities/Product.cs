using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NeckFinish { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CapacityVolume { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public Category Category { get; set; } = null!;

        public SubCategory SubCategory { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
