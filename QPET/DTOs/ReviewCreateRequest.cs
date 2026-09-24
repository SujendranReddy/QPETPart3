using System.ComponentModel.DataAnnotations;

namespace QPET.DTOs
{
    public class ReviewCreateRequest
    {
        [Range(1, int.MaxValue)]
        public int BranchId { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }
            = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string? EmailAddress { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string ReviewMessage { get; set; }
            = string.Empty;
    }
}