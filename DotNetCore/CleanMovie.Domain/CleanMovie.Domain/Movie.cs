using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMovie.Domain
{
    public class Movie
    {
        public int ID { get; set; }
        public string Name { get; set; } = default!;
        public decimal Cost { get; set; } = default!;
    }
}
