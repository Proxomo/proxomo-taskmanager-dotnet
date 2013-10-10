using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxomoApp.Models
{
    public class AuthenticateModel
    {
        public string AccessToken { get; set; }
        public string Expires { get; set; }
        public string PersonID { get; set; }
        public string Role { get; set; }
    }
}