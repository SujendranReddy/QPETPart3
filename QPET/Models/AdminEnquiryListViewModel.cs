namespace QPET.Models
{
    public class AdminEnquiryListViewModel
    {
        public IEnumerable<QPET.Data.Entities.Enquiry> Enquiries { get; set; }
            = new List<QPET.Data.Entities.Enquiry>();

        public string? SelectedStatus { get; set; }
    }
}