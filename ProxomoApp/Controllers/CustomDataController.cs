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
    public class CustomDataController : ApiController
    {
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
            catch (Exception e)
            {
                //catch any errors and store the exception
                response = e.Message;
            }

            AuthTokenModel authToken = JsonConvert.DeserializeObject<AuthTokenModel>(response);
            //return the results
            return authToken.accessToken;
        }

        //Custom data add
        [HttpGet]
        public string CustomDataAddRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string TABLE_NAME, string ID, string Transaction)
        {
            string url = "https://service.proxomo.com/v09/json/customdata";
            string ID_STRING = string.Empty;
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = authToken;

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //Create the json to send to the web service
                    string json = "{\"TableName\":\"" + TABLE_NAME + "\"," + "\"ID\":\"" + ID + "\",\"Transaction\":\"" + Transaction + "\"}";
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
            catch (Exception e)
            {
                //Return any exceptions that happened
                ID_STRING = e.Message;
            }

            //return the ID string
            return ID_STRING;

        }

        //Custom Data Get
        [HttpGet]
        public CustomDataModel CustomDataGetByIdRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string TABLE_NAME, string ID)
        {
            string url = "https://service.proxomo.com/v09/json/customdata/table/%TABLE_NAME%/%ID%";
            url = url.Replace("%TABLE_NAME%", TABLE_NAME).Replace("%ID%", ID);
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
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

            CustomDataModel customData = JsonConvert.DeserializeObject<CustomDataModel>(response);

            return customData;
        }

        [HttpDelete]
        public string CustomDataDeleteRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string TABLE_NAME, string ID)
        {
            string url = "https://service.proxomo.com/v09/json/customdata/table/%TABLE_NAME%/%ID%";
            url = url.Replace("%TABLE_NAME%", TABLE_NAME).Replace("%ID%", ID);
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Headers[HttpRequestHeader.Authorization] = authToken;

            string responseMessage = string.Empty;

            try
            {
                //Call the web service to retrieve your auth token
                var response = (HttpWebResponse)request.GetResponse();
                responseMessage = response.StatusCode.ToString();
            }
            catch (Exception e)
            {
                //catch any errors and store the exception
                responseMessage = e.Message;
            }

            return responseMessage;

        }

        [HttpGet]
        public string CustomDataSearchRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string TABLE_NAME, string QUERY, int MAX_RESULTS)
        {
            string url = "https://service.proxomo.com/v09/json/customdata/search/table/%TABLE_NAME%";
            url = url.Replace("%TABLE_NAME%", TABLE_NAME);
            url += "?q=" + QUERY + "&maxresults=" + MAX_RESULTS.ToString();

            string authtoken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = authtoken;
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

        [HttpPut]
        public string CustomDataUpdateRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string TABLE_NAME, string ID, string Transaction)
        {
            string url = "https://service.proxomo.com/v09/json/customdata";
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
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
                        string json = "{\"TableName\":\"" + TABLE_NAME + "\"," + "\"ID\":\"" + ID + "\",\"Transaction\":\"" + Transaction + "\"}";
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
