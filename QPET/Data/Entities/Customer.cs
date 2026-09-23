using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();
    }
}
