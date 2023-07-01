namespace WebAPI
{
    public class SuperHero
    {
        public int ID { get; set; }
        public string Name { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Place { get; set; } = default!;
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
