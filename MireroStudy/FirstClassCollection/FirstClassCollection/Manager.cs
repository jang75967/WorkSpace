namespace FirstClassCollection
{
    public class Manager
    {
        private Emails _emails { get; set; }
        public bool AddEmail(Email email)
        {
            return _emails.AddEmail(email);
        }
    }
}
