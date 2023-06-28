using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Models
{
    [Table(nameof(User))]
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
        public decimal Salary { get; set; } = default!;
    }
}
