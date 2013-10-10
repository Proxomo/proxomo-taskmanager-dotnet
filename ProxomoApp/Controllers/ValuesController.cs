using Newtonsoft.Json;
using ProxomoApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProxomoApp.Controllers
{
    public class ValuesController : ApiController
    {
        //Gets the authentication token
        [HttpHead]
        public string getAuthToken(string APPLICATION_ID, string PROXOMO_API_KEY)
        {
            //Create the URL along with your parameters
            string url = "https://service.proxomo.com/v09/json/security/accesstoken/get";
            url += "?applicationid=" + APPLICATION_ID + "&proxomoAPIKey=" + PROXOMO_API_KEY;

            //Created the method and set the request type
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string response = string.Empty;
            try
            {
                //Call the web service to retrieve your auth token
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);

                //read the result
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch(Exception e)
            {
                //catch any errors and store the exception
                response = e.Message;
            }

            AuthTokenModel authToken = JsonConvert.DeserializeObject<AuthTokenModel>(response);
            //return the results
            return authToken.accessToken;
        }

        //Add in app data
        [HttpGet]
        public string AppDataAdd(string APPLICATION_ID, string PROXOMO_API_KEY, string key, string value, string objectType)
        {
            //Create the URL
            string url = "https://service.proxomo.com/v09/json/appdata";
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string ID_STRING = string.Empty;

            //Create the method and set the request type
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = authToken;
            try
            {
               using (var streamWriter = new StreamWriter(request.GetRequestStream()))
               {
                   //Create the json to send to the web service
                   string json = "{\"Key\":\"" + key + "\"," + "\"Value\":\"" + value + "\",\"ObjectType\":\"" + objectType + "\"}";
                   streamWriter.Write(json);
                   streamWriter.Flush();
                   streamWriter.Close();

                   //Call the method and store the response
                   var response = (HttpWebResponse)request.GetResponse();

                   //read the response into a string
                   using (var streamReader = new StreamReader(response.GetResponseStream()))
                   {
                       ID_STRING = streamReader.ReadToEnd();
                   }
               }
            }
            catch(Exception e)
            {
                //Return any exceptions that happened
                ID_STRING = e.Message;
            }

            //return the ID string
            return ID_STRING;
        }

        //Retrieves app data by an id
        [HttpGet]
        public AppDataModel AppDataGet(string APPLICATION_ID, string PROXOMO_API_KEY, string APPDATA_ID)
        {
            string authtoken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/appdata/";
            url += APPDATA_ID;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = authtoken;
            //request.ContentType = "application/json";
            string response = string.Empty;

            try
            {
                //Call the web service to retrieve your auth token
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);

                //read the result
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (Exception e)
            {
                //catch any errors and store the exception
                response = e.Message;
            }

            AppDataModel appData = JsonConvert.DeserializeObject<AppDataModel>(response);

            return appData;
        }

        //deletes an app data by its ID
        [HttpDelete]
        public string AppDataDelete(string APPLICATION_ID, string PROXOMO_API_KEY, string APPDATA_ID)
        {
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            string url = "https://service.proxomo.com/v09/json/appdata/";
            url += APPDATA_ID;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Headers[HttpRequestHeader.Authorization] = authToken;
            string response = string.Empty;

            try
            {
                //Call the web service to retrieve your auth token
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);

                //read the result
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (Exception e)
            {
                //catch any errors and store the exception
                response = e.Message;
            }

            return response;
        }

        //Returns a list of all app data
        [HttpGet]
        public List<AppDataModel> GetAllAppData(string APPLICATION_ID, string PROXOMO_API_KEY)
        {
            string url = "https://service.proxomo.com/v09/json/appdata";
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = authToken;
            //request.ContentType = "application/json";
            string response = string.Empty;

            try
            {
                //Call the web service to retrieve your auth token
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);

                //read the result
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (Exception e)
            {
                //catch any errors and store the exception
                response = e.Message;
            }

            //serialize the object into a list of app data models
            List<AppDataModel> appData = JsonConvert.DeserializeObject<List<AppDataModel>>(response);

            return appData;
        }

        //Searches through app data by object Type
        [HttpGet]
        public List<AppDataModel> SearchAppData(string APPLICATION_ID, string PROXOMO_API_KEY, string OBJECT_TYPE)
        {
            string authtoken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/appdata/search/objecttype/";
            url += OBJECT_TYPE;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = authtoken;
            //request.ContentType = "application/json";
            string response = string.Empty;

            try
            {
                //Call the web service to retrieve your auth token
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);

                //read the result
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (Exception e)
            {
                //catch any errors and store the exception
                response = e.Message;
            }

            //serialize list of appdata
            List<AppDataModel> appData = JsonConvert.DeserializeObject<List<AppDataModel>>(response);

            return appData;
        }

        [HttpPut]
        public string AppDataUpdate(string APPLICATION_ID, string PROXOMO_API_KEY, string key, string value, string objectType)
        {
            string url = "https://service.proxomo.com/v09/json/appdata";
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = authToken;


            string responseMessage = string.Empty;
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //Create the json to send to the web service
                    string json = "{\"Key\":\"" + key + "\"," + "\"Value\":\"" + value + "\",\"ObjectType\":\"" + objectType + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    //Call the method and store the response
                    var response = (HttpWebResponse)request.GetResponse();
                    responseMessage = response.StatusCode.ToString();

                }
            }
            catch (Exception e)
            {
                responseMessage = e.Message;
            }
            return responseMessage;
        }
    }
}