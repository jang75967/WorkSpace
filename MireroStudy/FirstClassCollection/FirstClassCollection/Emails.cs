using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstClassCollection
{

    public class Emails
    {
        private readonly List<Email> _emails;

        public Emails(List<Email> emails)
        {
            _emails = emails;
        }

        public bool AddEmail(Email email)
        {
            if (_emails.Count >= 10)
                return false;

            if (_emails.Any(x => x.Equals(email)))
                return false;

            _emails.Add(email);
            return true;
        }

        public Email this[int index] { get => new Email(_emails[index].Local, _emails[index].Domain); }
    }
}
