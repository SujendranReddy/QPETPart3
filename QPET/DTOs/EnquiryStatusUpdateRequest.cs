using System.ComponentModel.DataAnnotations;

namespace QPET.DTOs
{
    public class EnquiryStatusUpdateRequest
    {
        [Required]
        public string Status { get; set; }
            = string.Empty;
    }
}