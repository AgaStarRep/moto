using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motohusaria.Web.Utils.Authorization
{
    public class JWTOptions
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
