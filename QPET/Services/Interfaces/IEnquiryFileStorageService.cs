using Microsoft.AspNetCore.Http;
using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IEnquiryFileStorageService
    {
        Task<EnquiryAttachment> SaveAsync(
            IFormFile file);

        string? GetFilePath(
            string storedFileName);
    }
}