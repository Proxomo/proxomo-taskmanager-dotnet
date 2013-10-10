using ProxomoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProxomoApp.Controllers
{
    public class HomeController : Controller
    {
        ProxomoApp.Controllers.ValuesController aData = new ProxomoApp.Controllers.ValuesController();
        static HomePageModel home = new HomePageModel();
        public ActionResult Index()
        {
            if (home.appID == null)
            {
                home.appID = string.Empty;
            }
            if (home.proxomoKey == null)
            {
                home.proxomoKey = string.Empty;
            }
            return View(home);
        }

        public ActionResult ShowList(FormCollection forms)
        {
            string apiKey = forms.Get("AppID");
            string proxomoKey = forms.Get("ProxomoKey");

            if (apiKey != null && proxomoKey != null && apiKey != string.Empty && proxomoKey != string.Empty)
            {
                string button = forms.Get("v");
                string value = forms.Get("value");
                string obj = forms.Get("object");
                string key = forms.Get("key");

                if (button == "Add")
                {
                    if (value != null && value != string.Empty && obj != null && obj != string.Empty && key != null && key != string.Empty)
                    {
                        aData.AppDataAdd(apiKey, proxomoKey, key, value, obj);
                    }
                    else
                    {
                        @ViewBag.error = "Please make sure your key, object type and value are filled in.";
                    }
                }

                home.appID = apiKey;
                home.proxomoKey = proxomoKey;
                home.appData = aData.GetAllAppData(apiKey, proxomoKey);
            }
            else
            {
                @ViewBag.error = "Please make sure both your api key and proxomo api key are filled in.";
            }
            
            

            return View("Index", home);
        }

        public int deleteItem(string appID, string proxomoKey, string itemID)
        {
            aData.AppDataDelete(appID, proxomoKey, itemID);
            return 200;
        }
    }
}
