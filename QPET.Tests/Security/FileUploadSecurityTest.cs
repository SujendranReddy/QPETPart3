using Microsoft.AspNetCore.Http;
using QPET.Security;

namespace QPET.Tests.Security
{
    public class FileUploadSecurityTests
    {
        [Fact]
        public void ValidateEnquiryAttachment_ValidPdf_ReturnsNull()
        {
            var bytes =
                new byte[]
                {
                    0x25,
                    0x50,
                    0x44,
                    0x46,
                    0x2D,
                    0x31,
                    0x2E,
                    0x37
                };

            var file =
                CreateFile(
                    bytes,
                    "document.pdf",
                    "application/pdf"
                );

            var result =
                FileUploadSecurity
                    .ValidateEnquiryAttachment(
                        file
                    );

            Assert.Null(result);
        }

        [Fact]
        public void ValidateEnquiryAttachment_InvalidExtension_ReturnsError()
        {
            var bytes =
                new byte[]
                {
                    0x01,
                    0x02,
                    0x03
                };

            var file =
                CreateFile(
                    bytes,
                    "dangerous.exe",
                    "application/octet-stream"
                );

            var result =
                FileUploadSecurity
                    .ValidateEnquiryAttachment(
                        file
                    );

            Assert.NotNull(result);
        }

        [Fact]
        public void ValidateEnquiryAttachment_FakePdf_ReturnsError()
        {
            var bytes =
                new byte[]
                {
                    0x4D,
                    0x5A,
                    0x90,
                    0x00,
                    0x03,
                    0x00
                };

            var file =
                CreateFile(
                    bytes,
                    "fake.pdf",
                    "application/pdf"
                );

            var result =
                FileUploadSecurity
                    .ValidateEnquiryAttachment(
                        file
                    );

            Assert.NotNull(result);
            Assert.Contains(
                "contents",
                result,
                StringComparison.OrdinalIgnoreCase
            );
        }

        [Fact]
        public void ValidateEnquiryAttachment_FileTooLarge_ReturnsError()
        {
            var file =
                new FormFile(
                    Stream.Null,
                    0,
                    FileUploadSecurity
                        .MaximumEnquiryAttachmentBytes + 1,
                    "file",
                    "large.pdf"
                )
                {
                    Headers =
                        new HeaderDictionary(),

                    ContentType =
                        "application/pdf"
                };

            var result =
                FileUploadSecurity
                    .ValidateEnquiryAttachment(
                        file
                    );

            Assert.NotNull(result);
            Assert.Contains(
                "5 MB",
                result
            );
        }

        [Fact]
        public void ValidateProductImage_ValidPng_ReturnsNull()
        {
            var bytes =
                new byte[]
                {
                    0x89,
                    0x50,
                    0x4E,
                    0x47,
                    0x0D,
                    0x0A,
                    0x1A,
                    0x0A
                };

            var file =
                CreateFile(
                    bytes,
                    "product.png",
                    "image/png"
                );

            var result =
                FileUploadSecurity
                    .ValidateProductImage(
                        file
                    );

            Assert.Null(result);
        }

        [Fact]
        public void ValidateProductImage_FakeJpg_ReturnsError()
        {
            var bytes =
                new byte[]
                {
                    0x25,
                    0x50,
                    0x44,
                    0x46,
                    0x2D
                };

            var file =
                CreateFile(
                    bytes,
                    "fake.jpg",
                    "image/jpeg"
                );

            var result =
                FileUploadSecurity
                    .ValidateProductImage(
                        file
                    );

            Assert.NotNull(result);
        }

        private static IFormFile CreateFile(
            byte[] bytes,
            string fileName,
            string contentType)
        {
            var stream =
                new MemoryStream(bytes);

            return new FormFile(
                stream,
                0,
                bytes.Length,
                "file",
                fileName
            )
            {
                Headers =
                    new HeaderDictionary(),

                ContentType =
                    contentType
            };
        }
    }
}