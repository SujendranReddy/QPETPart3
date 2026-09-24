using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services;

namespace QPET.Tests.Services
{
    public class AdminAuthServiceTests
    {
        [Fact]
        public async Task AuthenticateAsync_CorrectPassword_ReturnsAdmin()
        {
            await using var context =
                CreateContext();

            var hasher =
                new PasswordHasher<AdminUser>();

            var admin =
                new AdminUser
                {
                    EmailAddress =
                        "admin@qpet.co.za",

                    IsActive =
                        true
                };

            admin.PasswordHash =
                hasher.HashPassword(
                    admin,
                    "SecurePassword123!"
                );

            context.AdminUsers.Add(admin);

            await context.SaveChangesAsync();

            var service =
                new AdminAuthService(
                    context,
                    hasher
                );

            var result =
                await service.AuthenticateAsync(
                    "admin@qpet.co.za",
                    "SecurePassword123!"
                );

            Assert.NotNull(result);
            Assert.Equal(
                "admin@qpet.co.za",
                result.EmailAddress
            );
        }

        [Fact]
        public async Task AuthenticateAsync_WrongPassword_ReturnsNull()
        {
            await using var context =
                CreateContext();

            var hasher =
                new PasswordHasher<AdminUser>();

            var admin =
                new AdminUser
                {
                    EmailAddress =
                        "admin@qpet.co.za",

                    IsActive =
                        true
                };

            admin.PasswordHash =
                hasher.HashPassword(
                    admin,
                    "CorrectPassword123!"
                );

            context.AdminUsers.Add(admin);

            await context.SaveChangesAsync();

            var service =
                new AdminAuthService(
                    context,
                    hasher
                );

            var result =
                await service.AuthenticateAsync(
                    "admin@qpet.co.za",
                    "WrongPassword"
                );

            Assert.Null(result);
        }

        [Fact]
        public async Task AuthenticateAsync_InactiveAdmin_ReturnsNull()
        {
            await using var context =
                CreateContext();

            var hasher =
                new PasswordHasher<AdminUser>();

            var admin =
                new AdminUser
                {
                    EmailAddress =
                        "admin@qpet.co.za",

                    IsActive =
                        false
                };

            admin.PasswordHash =
                hasher.HashPassword(
                    admin,
                    "SecurePassword123!"
                );

            context.AdminUsers.Add(admin);

            await context.SaveChangesAsync();

            var service =
                new AdminAuthService(
                    context,
                    hasher
                );

            var result =
                await service.AuthenticateAsync(
                    "admin@qpet.co.za",
                    "SecurePassword123!"
                );

            Assert.Null(result);
        }

        private static QpetDbContext
            CreateContext()
        {
            var options =
                new DbContextOptionsBuilder<
                    QpetDbContext>()
                    .UseInMemoryDatabase(
                        Guid.NewGuid()
                            .ToString()
                    )
                    .Options;

            return new QpetDbContext(
                options
            );
        }
    }
}