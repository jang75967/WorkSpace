using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMovie.Domain
{
    public class Movie
    {
        //public int ID { get; set; }
        //public string Name { get; set; } = default!;
        //public decimal Cost { get; set; } = default!;

        public int MovieId { get; set; }
        public string MovieName { get; set; } = default!;
        public decimal RentalCost { get; set; }
        public int RentalDuration { get; set; }

        // n:m 관계
        public IList<MovieRental> MovieRentals { get; set;}
    }
}
