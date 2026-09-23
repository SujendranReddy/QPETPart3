using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IEnquiryService
    {
        Task<List<Customer>> GetCustomersAsync();

        Task<Customer?> FindCustomerByEmailAsync(
            string emailAddress);

        Task<Customer> AddCustomerAsync(
            Customer customer);

        Task<List<Enquiry>> GetEnquiriesAsync();

        Task<Enquiry?> GetEnquiryByIdAsync(
            int enquiryId);

        Task<Enquiry> AddEnquiryAsync(
            Enquiry enquiry);

        Task<bool> UpdateEnquiryStatusAsync(
            int enquiryId,
            string newStatus);
    }
}
