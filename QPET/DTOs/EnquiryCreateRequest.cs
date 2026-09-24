using System.ComponentModel.DataAnnotations;

namespace QPET.DTOs
{
    public class EnquiryCreateRequest
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
            = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string EmailAddress { get; set; }
            = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
            = string.Empty;

        [Range(1, int.MaxValue)]
        public int BranchId { get; set; }

        [Required]
        [StringLength(150)]
        public string Subject { get; set; }
            = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }
            = string.Empty;
    }
}