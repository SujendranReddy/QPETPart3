using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class AdminAuthService
        : IAdminAuthService
    {
        private readonly QpetDbContext _context;

        private readonly IPasswordHasher<AdminUser>
            _passwordHasher;

        public AdminAuthService(
            QpetDbContext context,
            IPasswordHasher<AdminUser> passwordHasher)
        {
            _context = context;

            _passwordHasher =
                passwordHasher;
        }

        public async Task<AdminUser?>
            AuthenticateAsync(
                string emailAddress,
                string password)
        {
            if (
                string.IsNullOrWhiteSpace(
                    emailAddress
                ) ||
                string.IsNullOrWhiteSpace(
                    password
                ))
            {
                return null;
            }

            var normalizedEmail =
                emailAddress
                    .Trim()
                    .ToLowerInvariant();

            var admin =
                await _context.AdminUsers
                    .FirstOrDefaultAsync(
                        admin =>
                            admin.EmailAddress ==
                                normalizedEmail &&
                            admin.IsActive
                    );

            if (admin == null)
            {
                return null;
            }

            var result =
                _passwordHasher
                    .VerifyHashedPassword(
                        admin,
                        admin.PasswordHash,
                        password
                    );

            if (
                result ==
                PasswordVerificationResult.Failed)
            {
                return null;
            }

            if (
                result ==
                PasswordVerificationResult
                    .SuccessRehashNeeded)
            {
                admin.PasswordHash =
                    _passwordHasher
                        .HashPassword(
                            admin,
                            password
                        );

                await _context
                    .SaveChangesAsync();
            }

            return admin;
        }
    }
}