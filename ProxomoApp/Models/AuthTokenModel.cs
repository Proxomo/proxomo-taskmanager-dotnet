using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxomoApp.Models
{
    public class AuthTokenModel
    {
        public string accessToken { get; set; }
        public long expires { get; set; }
    }
}