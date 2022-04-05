using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Options
{
    public class JwtSettings
    {
        public string SingningKey { get; set; }
        public string Issuer { get; set; }
        public string[] Audiences { get; set; }
    }
}
