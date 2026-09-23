namespace QPET.Models
{
    public class ReviewsPageViewModel
    {
        public List<QPET.Data.Entities.Review> Reviews { get; set; }
            = new List<QPET.Data.Entities.Review>();

        public List<QPET.Data.Entities.Branch> Branches { get; set; }
            = new List<QPET.Data.Entities.Branch>();

        public ReviewViewModel Review { get; set; }
            = new ReviewViewModel();
    }
}