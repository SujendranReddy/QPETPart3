using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public Product Product { get; set; } = null!;
    }
}
