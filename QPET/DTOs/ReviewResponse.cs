namespace QPET.DTOs
{
    public class ReviewResponse
    {
        public int ReviewId { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }
            = string.Empty;

        public string DisplayName { get; set; }
            = string.Empty;

        public int Rating { get; set; }

        public string ReviewMessage { get; set; }
            = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}