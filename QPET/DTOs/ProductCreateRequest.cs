using System.ComponentModel.DataAnnotations;

namespace QPET.DTOs
{
    public class ProductCreateRequest
    {
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int SubCategoryId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; }
            = string.Empty;

        [Required]
        [StringLength(100)]
        public string NeckFinish { get; set; }
            = string.Empty;

        [Required]
        [StringLength(50)]
        public string CapacityVolume { get; set; }
            = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
            = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}