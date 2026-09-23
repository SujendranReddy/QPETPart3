using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }

        [Required]
        [StringLength(100)]
        public string BranchName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Province { get; set; } = string.Empty;

        public ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
