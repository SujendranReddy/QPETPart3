
namespace QPET.Models
{
    public class AdminEnquiryListViewModel
    {
        public IEnumerable<QPET.Data.Entities.Enquiry> Enquiries
        { get; set; } =
            new List<QPET.Data.Entities.Enquiry>();

        public int TotalCount { get; set; }

        public string? Search { get; set; }

        public string? SelectedStatus { get; set; }

        public int? SelectedBranchId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? FilterError { get; set; }

        public bool HasActiveFilters =>
            !string.IsNullOrWhiteSpace(Search) ||
            !string.IsNullOrWhiteSpace(SelectedStatus) ||
            SelectedBranchId.HasValue ||
            FromDate.HasValue ||
            ToDate.HasValue;
    }
}
