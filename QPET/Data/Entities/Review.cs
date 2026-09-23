using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int BranchId { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string? EmailAddress { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string ReviewMessage { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public Branch Branch { get; set; } = null!;
    }
}
