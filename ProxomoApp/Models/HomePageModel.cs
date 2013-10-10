using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxomoApp.Models
{
    public class HomePageModel
    {
        public List<AppDataModel> appData { get; set; }
        public string appID { get; set; }
        public string proxomoKey { get; set; }
        public HomePageModel()
        {
            appData = new List<AppDataModel>();
        }
    }
}