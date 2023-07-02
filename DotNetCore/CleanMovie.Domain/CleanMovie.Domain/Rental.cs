namespace CleanMovie.Domain
{
    public class Rental
    {
        public int RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime RentalExpriy { get; set; }
        public decimal TotalCost { get; set; }

        // 1:n 관계
        public ICollection<Member> Members { get; set; }

        // n:m 관계
        public IList<MovieRental> MovieRentals { get; set; }
    }
}