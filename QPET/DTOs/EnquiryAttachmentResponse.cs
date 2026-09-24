namespace QPET.DTOs
{
    public class EnquiryAttachmentResponse
    {
        public int AttachmentId { get; set; }

        public string OriginalFileName { get; set; }
            = string.Empty;

        public string ContentType { get; set; }
            = string.Empty;
    }
}