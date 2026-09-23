using System.ComponentModel.DataAnnotations;

namespace QPET.Models
{
    public class ReviewViewModel
    {
        [Required(
            ErrorMessage = "Please enter a display name."
        )]
        [StringLength(100)]
        public string DisplayName { get; set; } =
            string.Empty;


        [EmailAddress(
            ErrorMessage = "Please enter a valid email address."
        )]
        [StringLength(150)]
        public string? EmailAddress { get; set; }


        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Please select a branch."
        )]
        public int BranchId { get; set; }


        [Range(
            1,
            5,
            ErrorMessage = "Please select a rating."
        )]
        public int Rating { get; set; }


        [Required(
            ErrorMessage = "Please enter your review."
        )]
        [StringLength(1000)]
        public string ReviewMessage { get; set; } =
            string.Empty;
    }
}