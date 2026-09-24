using Microsoft.AspNetCore.Http;
using QPET.Security;
using System.ComponentModel.DataAnnotations;

namespace QPET.Models
{
    public class EnquiryViewModel
        : IValidatableObject
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Please select a branch."
        )]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required(
            ErrorMessage =
                "Please enter your full name."
        )]
        [StringLength(
            120,
            ErrorMessage =
                "Full name cannot exceed 120 characters."
        )]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Please enter your email address."
        )]
        [EmailAddress(
            ErrorMessage =
                "Please enter a valid email address."
        )]
        [StringLength(
            150,
            ErrorMessage =
                "Email address cannot exceed 150 characters."
        )]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Please enter your phone number."
        )]
        [Phone(
            ErrorMessage =
                "Please enter a valid phone number."
        )]
        [StringLength(
            30,
            ErrorMessage =
                "Phone number cannot exceed 30 characters."
        )]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Please enter an enquiry subject."
        )]
        [StringLength(
            150,
            ErrorMessage =
                "Subject cannot exceed 150 characters."
        )]
        public string Subject { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Please enter your enquiry message."
        )]
        [StringLength(
            2000,
            ErrorMessage =
                "Message cannot exceed 2000 characters."
        )]
        public string Message { get; set; }
            = string.Empty;

        public List<IFormFile> Attachments
        {
            get;
            set;
        } = new();

        public IEnumerable<ValidationResult>
            Validate(
                ValidationContext validationContext)
        {
            if (
                Attachments.Count >
                FileUploadSecurity
                    .MaximumEnquiryAttachmentCount)
            {
                yield return
                    new ValidationResult(
                        "A maximum of 5 attachments is allowed.",
                        new[]
                        {
                            nameof(Attachments)
                        }
                    );

                yield break;
            }

            foreach (
                var attachment
                in Attachments)
            {
                var error =
                    FileUploadSecurity
                        .ValidateEnquiryAttachment(
                            attachment
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
                                nameof(
                                    Attachments
                                )
                            }
                        );
                }
            }
        }
    }
}