using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class EnquiryAttachment
    {
        [Key]
        public int AttachmentId { get; set; }

        public int EnquiryId { get; set; }

        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string StoredFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public Enquiry Enquiry { get; set; } = null!;
    }
}
