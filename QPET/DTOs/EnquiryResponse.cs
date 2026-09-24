namespace QPET.DTOs
{
    public class EnquiryResponse
    {
        public int EnquiryId { get; set; }

        public string CustomerName { get; set; }
            = string.Empty;

        public string CustomerEmail { get; set; }
            = string.Empty;

        public string CustomerPhoneNumber { get; set; }
            = string.Empty;

        public int BranchId { get; set; }

        public string BranchName { get; set; }
            = string.Empty;

        public string Subject { get; set; }
            = string.Empty;

        public string Message { get; set; }
            = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }
            = string.Empty;

        public List<EnquiryAttachmentResponse>
            Attachments
        { get; set; }
            = new List<EnquiryAttachmentResponse>();
    }
}