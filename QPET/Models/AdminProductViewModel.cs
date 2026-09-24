using Microsoft.AspNetCore.Http;
using QPET.Security;
using System.ComponentModel.DataAnnotations;

namespace QPET.Models
{
    public class AdminProductViewModel
        : IValidatableObject
    {
        public int? ProductId { get; set; }

        [Required(
            ErrorMessage =
                "Please enter a product name."
        )]
        [StringLength(
            150,
            ErrorMessage =
                "Product name cannot exceed 150 characters."
        )]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
            = string.Empty;

        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Please select a category."
        )]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(
            ErrorMessage =
                "Please select a subcategory."
        )]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Please select a valid subcategory."
        )]
        [Display(Name = "Subcategory")]
        public int? SubCategoryId { get; set; }

        [Required(
            ErrorMessage =
                "Please enter the neck finish."
        )]
        [StringLength(
            100,
            ErrorMessage =
                "Neck finish cannot exceed 100 characters."
        )]
        [Display(Name = "Neck Finish")]
        public string NeckFinish { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Please enter the capacity or volume."
        )]
        [StringLength(
            50,
            ErrorMessage =
                "Capacity or volume cannot exceed 50 characters."
        )]
        [Display(Name = "Capacity / Volume")]
        public string CapacityVolume { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Please enter a description."
        )]
        [StringLength(
            1000,
            ErrorMessage =
                "Description cannot exceed 1000 characters."
        )]
        public string Description { get; set; }
            = string.Empty;

        [Display(Name = "Product Image")]
        public IFormFile? Image { get; set; }

        public string? ExistingImageUrl
        {
            get;
            set;
        }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult>
            Validate(
                ValidationContext validationContext)
        {
            if (Image == null)
            {
                yield break;
            }

            var error =
                FileUploadSecurity
                    .ValidateProductImage(
                        Image
                    );

            if (
                !string.IsNullOrWhiteSpace(
                    error
                ))
            {
                yield return
                    new ValidationResult(
                        error,
                        new[]
                        {
                            nameof(Image)
                        }
                    );
            }
        }
    }
}