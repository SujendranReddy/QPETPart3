using Microsoft.AspNetCore.Http;
using QPET.Models;
using System.ComponentModel.DataAnnotations;

namespace QPET.Tests.Models
{
    public class ModelValidationTests
    {
        [Fact]
        public void EnquiryViewModel_MoreThanFiveAttachments_IsInvalid()
        {
            var model =
                new EnquiryViewModel
                {
                    BranchId = 1,
                    FullName = "Test Customer",
                    EmailAddress = "test@example.com",
                    PhoneNumber = "0821234567",
                    Subject = "Test enquiry",
                    Message = "Test message"
                };

            for (
                var index = 0;
                index < 6;
                index++)
            {
                model.Attachments.Add(
                    CreateFile(
                        "attachment.pdf",
                        "application/pdf"
                    )
                );
            }

            var validationResults =
                Validate(model);

            Assert.Contains(
                validationResults,
                result =>
                    result.ErrorMessage != null &&
                    result.ErrorMessage.Contains(
                        "maximum of 5",
                        StringComparison.OrdinalIgnoreCase
                    )
            );
        }

        [Fact]
        public void EnquiryViewModel_InvalidEmail_IsInvalid()
        {
            var model =
                new EnquiryViewModel
                {
                    BranchId = 1,
                    FullName = "Test Customer",
                    EmailAddress = "not-an-email",
                    PhoneNumber = "0821234567",
                    Subject = "Test enquiry",
                    Message = "Test message"
                };

            var results =
                Validate(model);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void AdminProductViewModel_InvalidImage_IsInvalid()
        {
            var model =
                new AdminProductViewModel
                {
                    ProductName = "Test Bottle",
                    CategoryId = 1,
                    SubCategoryId = 1,
                    NeckFinish = "28mm",
                    CapacityVolume = "500ml",
                    Description = "Test product",
                    IsActive = true,
                    Image =
                        CreateFile(
                            "malware.exe",
                            "application/octet-stream"
                        )
                };

            var results =
                Validate(model);

            Assert.Contains(
                results,
                result =>
                    result.MemberNames.Contains(
                        nameof(
                            AdminProductViewModel
                                .Image
                        )
                    )
            );
        }

        [Fact]
        public void AdminProductViewModel_ValidFieldsWithoutImage_IsValid()
        {
            var model =
                new AdminProductViewModel
                {
                    ProductName = "Test Bottle",
                    CategoryId = 1,
                    SubCategoryId = 1,
                    NeckFinish = "28mm",
                    CapacityVolume = "500ml",
                    Description = "Test product",
                    IsActive = true
                };

            var results =
                Validate(model);

            Assert.Empty(results);
        }

        private static List<ValidationResult>
            Validate(
                object model)
        {
            var results =
                new List<ValidationResult>();

            var context =
                new ValidationContext(model);

            Validator.TryValidateObject(
                model,
                context,
                results,
                true
            );

            return results;
        }

        private static IFormFile CreateFile(
            string fileName,
            string contentType)
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