using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCTest.Models
{
    [Table("Author")]
    public class Author
    {
        [Key]
        public string quote { get; set; } = default!;
        public string author { get; set; } = default!;
        public string more1 { get; set; } = default!;
        public string more2 { get; set; } = default!;
    }
}
