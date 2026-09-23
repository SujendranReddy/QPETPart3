using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class EnquiryFileStorageService
        : IEnquiryFileStorageService
    {
        private readonly string _storagePath;

        public EnquiryFileStorageService(
            IWebHostEnvironment environment)
        {
            _storagePath =
                Path.Combine(
                    environment.ContentRootPath,
                    "Data",
                    "EnquiryAttachments"
                );

            Directory.CreateDirectory(
                _storagePath
            );
        }

        public async Task<EnquiryAttachment> SaveAsync(
            IFormFile file)
        {
            if (file.Length <= 0)
            {
                throw new ArgumentException(
                    "The uploaded file is empty."
                );
            }

            var originalFileName =
                Path.GetFileName(
                    file.FileName
                );

            var extension =
                Path.GetExtension(
                    originalFileName
                )
                .ToLowerInvariant();

            var storedFileName =
                $"{Guid.NewGuid():N}{extension}";

            var filePath =
                Path.Combine(
                    _storagePath,
                    storedFileName
                );

            await using var stream =
                new FileStream(
                    filePath,
                    FileMode.CreateNew
                );

            await file.CopyToAsync(stream);

            return new EnquiryAttachment
            {
                OriginalFileName =
                    originalFileName,

                StoredFileName =
                    storedFileName,

                ContentType =
                    string.IsNullOrWhiteSpace(
                        file.ContentType
                    )
                        ? "application/octet-stream"
                        : file.ContentType
            };
        }

        public string? GetFilePath(
            string storedFileName)
        {
            if (string.IsNullOrWhiteSpace(
                storedFileName))
            {
                return null;
            }

            var safeFileName =
                Path.GetFileName(
                    storedFileName
                );

            var filePath =
                Path.Combine(
                    _storagePath,
                    safeFileName
                );

            return File.Exists(filePath)
                ? filePath
                : null;
        }
    }
}