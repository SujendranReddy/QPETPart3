using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;

namespace QPET.Tests.Data
{
    public class DatabasePersistenceTests
    {
        [Fact]
        public async Task Review_CanBeStoredAndRetrievedWithBranch()
        {
            await using var context =
                CreateContext();

            var branch =
                new Branch
                {
                    BranchName =
                        "Test Branch",

                    PhoneNumber =
                        "0331234567",

                    EmailAddress =
                        "branch@test.co.za",

                    City =
                        "Pietermaritzburg",

                    Province =
                        "KwaZulu-Natal"
                };

            context.Branches.Add(
                branch
            );

            await context
                .SaveChangesAsync();

            var review =
                new Review
                {
                    BranchId =
                        branch.BranchId,

                    DisplayName =
                        "Test Customer",

                    EmailAddress =
                        "customer@test.com",

                    Rating =
                        5,

                    ReviewMessage =
                        "Excellent service",

                    CreatedDate =
                        DateTime.UtcNow
                };

            context.Reviews.Add(
                review
            );

            await context
                .SaveChangesAsync();

            var stored =
                await context.Reviews
                    .Include(
                        current =>
                            current.Branch
                    )
                    .SingleAsync();

            Assert.Equal(
                "Test Customer",
                stored.DisplayName
            );

            Assert.NotNull(
                stored.Branch
            );

            Assert.Equal(
                "Test Branch",
                stored.Branch.BranchName
            );
        }

        [Fact]
        public async Task Enquiry_CanBeStoredWithCustomerAndBranch()
        {
            await using var context =
                CreateContext();

            var branch =
                new Branch
                {
                    BranchName =
                        "Test Branch",

                    PhoneNumber =
                        "0331234567",

                    EmailAddress =
                        "branch@test.co.za",

                    City =
                        "Pietermaritzburg",

                    Province =
                        "KwaZulu-Natal"
                };

            var customer =
                new Customer
                {
                    FullName =
                        "Test Customer",

                    EmailAddress =
                        "customer@test.com",

                    PhoneNumber =
                        "0821234567"
                };

            context.Branches.Add(
                branch
            );

            context.Customers.Add(
                customer
            );

            await context
                .SaveChangesAsync();

            var enquiry =
                new Enquiry
                {
                    CustomerId =
                        customer.CustomerId,

                    BranchId =
                        branch.BranchId,

                    Subject =
                        "Product enquiry",

                    Message =
                        "Please send more information.",

                    CreatedDate =
                        DateTime.UtcNow,

                    Status =
                        "New"
                };

            context.Enquiries.Add(
                enquiry
            );

            await context
                .SaveChangesAsync();

            var stored =
                await context.Enquiries
                    .Include(
                        current =>
                            current.Customer
                    )
                    .Include(
                        current =>
                            current.Branch
                    )
                    .SingleAsync();

            Assert.Equal(
                "New",
                stored.Status
            );

            Assert.Equal(
                "Test Customer",
                stored.Customer.FullName
            );

            Assert.Equal(
                "Test Branch",
                stored.Branch.BranchName
            );
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