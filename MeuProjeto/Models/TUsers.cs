using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeuProjeto.Models
{
    public class TUsers
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FullName { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Country { get; set; }
        public virtual string Role { get; set; }

    }
}