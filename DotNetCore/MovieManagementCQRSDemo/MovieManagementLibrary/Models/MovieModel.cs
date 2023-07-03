namespace MovieManagementLibrary.Models
{
    public class MovieModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Cost { get; set; }
    }
}