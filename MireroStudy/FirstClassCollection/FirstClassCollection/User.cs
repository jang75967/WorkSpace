namespace FirstClassCollection
{
    public class User
    {
        private Emails _emails { get; set; }

        public bool AddEmail(Email email)
        {
            return _emails.AddEmail(email);
        }
    }
}
