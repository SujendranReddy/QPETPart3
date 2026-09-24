using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IAdminAuthService
    {
        Task<AdminUser?> AuthenticateAsync(
            string emailAddress,
            string password);
    }
}