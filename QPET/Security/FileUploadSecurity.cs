using Microsoft.AspNetCore.Http;

namespace QPET.Security
{
    public static class FileUploadSecurity
    {
        public const int MaximumEnquiryAttachmentCount = 5;

        public const long MaximumEnquiryAttachmentBytes =
            5 * 1024 * 1024;

        public const long MaximumProductImageBytes =
            5 * 1024 * 1024;

        private static readonly HashSet<string>
            EnquiryExtensions =
                new(
                    StringComparer.OrdinalIgnoreCase
                )
                {
                    ".pdf",
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

        private static readonly HashSet<string>
            ProductImageExtensions =
                new(
                    StringComparer.OrdinalIgnoreCase
                )
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

        public static string?
            ValidateEnquiryAttachment(
                IFormFile file)
        {
            if (file == null)
            {
                return "The attachment is invalid.";
            }

            if (file.Length <= 0)
            {
                return "Uploaded files cannot be empty.";
            }

            if (
                file.Length >
                MaximumEnquiryAttachmentBytes)
            {
                return "Each attachment must be 5 MB or smaller.";
            }

            var extension =
                Path.GetExtension(
                    file.FileName
                )
                .ToLowerInvariant();

            if (
                string.IsNullOrWhiteSpace(
                    extension
                ) ||
                !EnquiryExtensions.Contains(
                    extension
                ))
            {
                return "Only PDF, JPG, JPEG and PNG attachments are allowed.";
            }

            if (
                !HasExpectedContentType(
                    file,
                    extension
                ))
            {
                return "The attachment content type does not match the selected file type.";
            }

            if (
                !HasValidFileSignature(
                    file,
                    extension
                ))
            {
                return "The attachment contents do not match its file extension.";
            }

            return null;
        }

        public static string?
            ValidateProductImage(
                IFormFile file)
        {
            if (file == null)
            {
                return "The image is invalid.";
            }

            if (file.Length <= 0)
            {
                return "Uploaded images cannot be empty.";
            }

            if (
                file.Length >
                MaximumProductImageBytes)
            {
                return "Product images must be 5 MB or smaller.";
            }

            var extension =
                Path.GetExtension(
                    file.FileName
                )
                .ToLowerInvariant();

            if (
                string.IsNullOrWhiteSpace(
                    extension
                ) ||
                !ProductImageExtensions.Contains(
                    extension
                ))
            {
                return "Only JPG, JPEG, PNG and WEBP images are allowed.";
            }

            if (
                !HasExpectedContentType(
                    file,
                    extension
                ))
            {
                return "The image content type does not match the selected file type.";
            }

            if (
                !HasValidFileSignature(
                    file,
                    extension
                ))
            {
                return "The image contents do not match its file extension.";
            }

            return null;
        }

        public static string
            GetTrustedContentType(
                string extension)
        {
            return extension
                .ToLowerInvariant()
                switch
            {
                ".pdf" =>
                    "application/pdf",

                ".jpg" =>
                    "image/jpeg",

                ".jpeg" =>
                    "image/jpeg",

                ".png" =>
                    "image/png",

                ".webp" =>
                    "image/webp",

                _ =>
                    "application/octet-stream"
            };
        }

        private static bool
            HasExpectedContentType(
                IFormFile file,
                string extension)
        {
            if (
                string.IsNullOrWhiteSpace(
                    file.ContentType
                ) ||
                file.ContentType.Equals(
                    "application/octet-stream",
                    StringComparison.OrdinalIgnoreCase
                ))
            {
                return true;
            }

            var expected =
                GetTrustedContentType(
                    extension
                );

            return file.ContentType.Equals(
                expected,
                StringComparison.OrdinalIgnoreCase
            );
        }

        private static bool
            HasValidFileSignature(
                IFormFile file,
                string extension)
        {
            try
            {
                using var stream =
                    file.OpenReadStream();

                var header =
                    new byte[12];

                var bytesRead =
                    stream.Read(
                        header,
                        0,
                        header.Length
                    );

                return extension switch
                {
                    ".pdf" =>
                        IsPdf(
                            header,
                            bytesRead
                        ),

                    ".jpg" =>
                        IsJpeg(
                            header,
                            bytesRead
                        ),

                    ".jpeg" =>
                        IsJpeg(
                            header,
                            bytesRead
                        ),

                    ".png" =>
                        IsPng(
                            header,
                            bytesRead
                        ),

                    ".webp" =>
                        IsWebP(
                            header,
                            bytesRead
                        ),

                    _ =>
                        false
                };
            }
            catch
            {
                return false;
            }
        }

        private static bool IsPdf(
            byte[] header,
            int bytesRead)
        {
            return
                bytesRead >= 5 &&
                header[0] == 0x25 &&
                header[1] == 0x50 &&
                header[2] == 0x44 &&
                header[3] == 0x46 &&
                header[4] == 0x2D;
        }

        private static bool IsJpeg(
            byte[] header,
            int bytesRead)
        {
            return
                bytesRead >= 3 &&
                header[0] == 0xFF &&
                header[1] == 0xD8 &&
                header[2] == 0xFF;
        }

        private static bool IsPng(
            byte[] header,
            int bytesRead)
        {
            return
                bytesRead >= 8 &&
                header[0] == 0x89 &&
                header[1] == 0x50 &&
                header[2] == 0x4E &&
                header[3] == 0x47 &&
                header[4] == 0x0D &&
                header[5] == 0x0A &&
                header[6] == 0x1A &&
                header[7] == 0x0A;
        }

        private static bool IsWebP(
            byte[] header,
            int bytesRead)
        {
            return
                bytesRead >= 12 &&
                header[0] == 0x52 &&
                header[1] == 0x49 &&
                header[2] == 0x46 &&
                header[3] == 0x46 &&
                header[8] == 0x57 &&
                header[9] == 0x45 &&
                header[10] == 0x42 &&
                header[11] == 0x50;
        }
    }
}