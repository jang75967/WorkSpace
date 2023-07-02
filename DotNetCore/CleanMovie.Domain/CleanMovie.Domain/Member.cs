using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMovie.Domain
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstNmae { get; set; } = default!;
        public string LastNmae { get; set; } = default!;
        public string Email { get; set; } = default!;

        // 1:n 연결
        public int RentalId { get; set; }
        public Rental Rental { get; set; }
    }
}
