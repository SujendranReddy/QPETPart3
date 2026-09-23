namespace QPET.Models
{
    public class ReviewsPageViewModel
    {
        public List<Review> Reviews { get; set; } =
            new List<Review>();

        public List<Branch> Branches { get; set; } =
            new List<Branch>();

        public ReviewViewModel Review { get; set; } =
            new ReviewViewModel();
    }
}