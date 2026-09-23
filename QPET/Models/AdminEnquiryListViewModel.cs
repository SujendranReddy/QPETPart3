namespace QPET.Models
{
    public class AdminEnquiryListViewModel
    {
        public IEnumerable<Enquiry> Enquiries { get; set; } =
            new List<Enquiry>();

        public string? SelectedStatus { get; set; }
    }
}