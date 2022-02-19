using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeuProjeto.Models
{
    public sealed class Alert
    {
        public String Type { get; set; }
        public String Message { get; set; }

        public Alert(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}