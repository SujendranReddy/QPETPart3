using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QPET.Data;

namespace QPET.Tests.Infrastructure
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.UseEnvironment(
                "Development"
            );

            builder.ConfigureServices(
                services =>
                {
                    services.RemoveAll<
                        QpetDbContext>();

                    services.RemoveAll<
                        DbContextOptions<
                            QpetDbContext>>();

                    services.RemoveAll<
                        IDbContextOptionsConfiguration<
                            QpetDbContext>>();

                    services.AddDbContext<
                        QpetDbContext>(
                        options =>
                        {
                            options
                                .UseInMemoryDatabase(
                                    "QPETIntegrationTests"
                                );
                        }
                    );
                }
            );
        }
    }
}