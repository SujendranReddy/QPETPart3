using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class Enquiry
    {
        public int EnquiryId { get; set; }

        public int CustomerId { get; set; }

        public int BranchId { get; set; }

        [Required]
        [StringLength(150)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        public Customer Customer { get; set; } = null!;

        public Branch Branch { get; set; } = null!;

        public ICollection<EnquiryAttachment> Attachments { get; set; } = new List<EnquiryAttachment>();
    }
}
