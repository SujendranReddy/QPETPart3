using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QPET.Data.Entities;

namespace QPET.Data
{
    public class AdminAccountSeeder
    {
        private readonly QpetDbContext _context;

        private readonly IPasswordHasher<AdminUser>
            _passwordHasher;

        private readonly IConfiguration
            _configuration;

        public AdminAccountSeeder(
            QpetDbContext context,
            IPasswordHasher<AdminUser> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;

            _passwordHasher =
                passwordHasher;

            _configuration =
                configuration;
        }

        public async Task SeedAsync()
        {
            var email =
                _configuration[
                    "AdminSeed:Email"
                ]
                ?.Trim()
                .ToLowerInvariant();

            var password =
                _configuration[
                    "AdminSeed:Password"
                ];

            if (
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var existingAdmin =
                await _context.AdminUsers
                    .FirstOrDefaultAsync(
                        admin =>
                            admin.EmailAddress ==
                            email
                    );

            if (existingAdmin != null)
            {
                return;
            }

            var admin =
                new AdminUser
                {
                    EmailAddress = email,
                    IsActive = true
                };

            admin.PasswordHash =
                _passwordHasher
                    .HashPassword(
                        admin,
                        password
                    );

            await _context.AdminUsers
                .AddAsync(admin);

            await _context
                .SaveChangesAsync();
        }
    }
}