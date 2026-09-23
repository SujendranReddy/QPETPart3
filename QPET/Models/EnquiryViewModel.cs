using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QPET.Models
{
    public class EnquiryViewModel
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Please select a branch."
        )]
        public int BranchId { get; set; }

        [Required(
            ErrorMessage = "Please enter your full name."
        )]
        [StringLength(100)]
        public string FullName { get; set; } =
            string.Empty;

        [Required(
            ErrorMessage = "Please enter your email address."
        )]
        [EmailAddress(
            ErrorMessage = "Please enter a valid email address."
        )]
        [StringLength(150)]
        public string EmailAddress { get; set; } =
            string.Empty;

        [Required(
            ErrorMessage = "Please enter your phone number."
        )]
        [StringLength(20)]
        public string PhoneNumber { get; set; } =
            string.Empty;

        [Required(
            ErrorMessage = "Please enter an enquiry subject."
        )]
        [StringLength(150)]
        public string Subject { get; set; } =
            string.Empty;

        [Required(
            ErrorMessage = "Please enter your enquiry message."
        )]
        [StringLength(2000)]
        public string Message { get; set; } =
            string.Empty;

        public List<IFormFile> Attachments { get; set; } =
            new List<IFormFile>();
    }
}