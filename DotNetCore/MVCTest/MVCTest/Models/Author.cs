using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCTest.Models
{
    // json 파일 파싱하기 위한 모델

    //[Table("AUTHOR")]
    //public class Author
    //{
    //    [Key]
    //    public string quote { get; set; } = default!;
    //    public string author { get; set; } = default!;
    //}

    // 오라클에서 사용하는 스키마

    [Table("AUTHOR")]
    public class Author
    {
        [Key]
        public string DB_QUOTE { get; set; } = default!;
        public string DB_AUTHOR { get; set; } = default!;
        public string DB_MORE1 { get; set; } = default!;
        public string DB_MORE2 { get; set; } = default!;
    }
}
