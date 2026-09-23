using System.ComponentModel.DataAnnotations;

namespace QPET.Data.Entities
{
    public class AdminUser
    {
        public int AdminUserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
