using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly QpetDbContext _context;

        public EnquiryService(QpetDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderBy(customer => customer.FullName)
                .ToListAsync();
        }

        public async Task<Customer?> FindCustomerByEmailAsync(
            string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return null;
            }

            var normalizedEmail =
                emailAddress.Trim();

            return await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    customer =>
                        customer.EmailAddress ==
                        normalizedEmail
                );
        }

        public async Task<Customer> AddCustomerAsync(
            Customer customer)
        {
            var existingCustomer =
                await FindCustomerByEmailAsync(
                    customer.EmailAddress
                );

            if (existingCustomer != null)
            {
                return existingCustomer;
            }

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<List<Enquiry>> GetEnquiriesAsync()
        {
            return await _context.Enquiries
                .AsNoTracking()
                .Include(enquiry => enquiry.Customer)
                .Include(enquiry => enquiry.Branch)
                .Include(enquiry => enquiry.Attachments)
                .OrderByDescending(
                    enquiry =>
                        enquiry.EnquiryId
                )
                .ToListAsync();
        }

        public async Task<Enquiry?> GetEnquiryByIdAsync(
            int enquiryId)
        {
            if (enquiryId <= 0)
            {
                return null;
            }

            return await _context.Enquiries
                .AsNoTracking()
                .Include(enquiry => enquiry.Customer)
                .Include(enquiry => enquiry.Branch)
                .Include(enquiry => enquiry.Attachments)
                .FirstOrDefaultAsync(
                    enquiry =>
                        enquiry.EnquiryId ==
                        enquiryId
                );
        }

        public async Task<Enquiry> AddEnquiryAsync(
            Enquiry enquiry)
        {
            var validBranch =
                await _context.Branches
                    .AnyAsync(
                        branch =>
                            branch.BranchId ==
                            enquiry.BranchId
                    );

            var validCustomer =
                await _context.Customers
                    .AnyAsync(
                        customer =>
                            customer.CustomerId ==
                            enquiry.CustomerId
                    );

            if (!validBranch || !validCustomer)
            {
                throw new ArgumentException(
                    "The enquiry contains an invalid customer or branch."
                );
            }

            enquiry.Status = "New";
            enquiry.CreatedDate = DateTime.Now;

            await _context.Enquiries.AddAsync(enquiry);
            await _context.SaveChangesAsync();

            return enquiry;
        }

        public async Task<bool> UpdateEnquiryStatusAsync(
            int enquiryId,
            string newStatus)
        {
            var enquiry =
                await _context.Enquiries
                    .FirstOrDefaultAsync(
                        enquiry =>
                            enquiry.EnquiryId ==
                            enquiryId
                    );

            if (enquiry == null)
            {
                return false;
            }

            var allowedTransitions =
                new Dictionary<string, string?>
                {
                    { "New", "InProgress" },
                    { "InProgress", "Resolved" },
                    { "Resolved", "Closed" },
                    { "Closed", null }
                };

            if (
                !allowedTransitions.TryGetValue(
                    enquiry.Status,
                    out var allowedStatus
                ) ||
                allowedStatus != newStatus)
            {
                return false;
            }

            enquiry.Status = newStatus;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EnquiryAttachment?>
    GetAttachmentByIdAsync(
        int attachmentId)
        {
            if (attachmentId <= 0)
            {
                return null;
            }

            return await _context
                .EnquiryAttachments
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    attachment =>
                        attachment.AttachmentId ==
                        attachmentId
                );
        }
    }
}
