using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QPET.Models
{
    public class AdminProductViewModel
    {
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Enter a product name.")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Select a category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Select a subcategory.")]
        public int? SubCategoryId { get; set; }

        [Required(ErrorMessage = "Enter the neck finish.")]
        [StringLength(100)]
        public string NeckFinish { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the capacity or volume.")]
        [StringLength(100)]
        public string CapacityVolume { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a product description.")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public string? ExistingImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}