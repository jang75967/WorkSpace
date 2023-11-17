using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstClassCollection
{
    public class Email
    {
        public string Local { get; set; }
        public string Domain { get; set; }

        public Email(string local, string domain)
        {
            Local = local;
            Domain = domain;
        }

        public override int GetHashCode()
        {
            return Local.GetHashCode() + Domain.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Email o = obj as Email;
            return o != null && (o.Local.Equals(this.Local)) && (o.Domain.Equals(this.Domain));
        }
    }
}
