namespace QPET.Models
{

    //This contains the summary data needed to display the admin dashboaord 
    public class AdminDashboardViewModel
    {
        public int ActiveProductCount { get; set; }

        public int NewEnquiryCount { get; set; }

        public int OpenEnquiryCount { get; set; }

        public int ReviewCount { get; set; }

        public List<AdminDashboardEnquiryViewModel>
            RecentEnquiries
        { get; set; } =
                new List<AdminDashboardEnquiryViewModel>();
    }


    public class AdminDashboardEnquiryViewModel
    {
        public int EnquiryId { get; set; }

        public string CustomerName { get; set; } =
            string.Empty;

        public string CustomerEmail { get; set; } =
            string.Empty;

        public string BranchName { get; set; } =
            string.Empty;

        public string Subject { get; set; } =
            string.Empty;

        public string CreatedDate { get; set; } =
            string.Empty;

        public string Status { get; set; } =
            string.Empty;
    }
}