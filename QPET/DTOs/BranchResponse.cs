namespace QPET.DTOs
{
    public class BranchResponse
    {
        public int BranchId { get; set; }

        public string BranchName { get; set; }
            = string.Empty;

        public string PhoneNumber { get; set; }
            = string.Empty;

        public string EmailAddress { get; set; }
            = string.Empty;

        public string City { get; set; }
            = string.Empty;

        public string Province { get; set; }
            = string.Empty;
    }
}