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
    public class PersonController : ApiController
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

        //Creates person for your aplication
        [HttpPost]
        public PersonsModel PersonCreateRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string USER_NAME, string PASSWORD, string ROLE)
        {
            string url = "https://service.proxomo.com/v09/json/security/person/create?";
            url += "username=" + USER_NAME + "&password=" + PASSWORD + "&role=" + ROLE;

            //Aquire the authToken and initiate the method.
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = authToken;
            string returnString = string.Empty;

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //Create the json to send to the web service
                    string json = "{\"USER_NAME\":\"" + USER_NAME + "\"," + "\"PASSWORD\":\"" + PASSWORD + "\",\"ROLE\":\"" + ROLE + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    //Call the method and store the response
                    var response = (HttpWebResponse)request.GetResponse();

                    //read the response into a string
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        returnString = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                //catch any errors and store the exception
                returnString = e.Message;
            }

            PersonsModel person = JsonConvert.DeserializeObject<PersonsModel>(returnString);
            //return the results
            return person;
        }

        //Returns list of all persons for an app
        [HttpGet]
        public List<PersonsModel> PersonAppGetRequest(string APPLICATION_ID, string PROXOMO_API_KEY)
        {
            //Create the URL along with your parameters
            string url = "https://service.proxomo.com/v09/json/security/persons";
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Created the method and set the request type
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

            List<PersonsModel> person = JsonConvert.DeserializeObject<List<PersonsModel>>(response);
            //return the results
            return person;
        }

        //Delets a person
        [HttpDelete]
        public string PersonDeleteRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID)
        {
            string authtoken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/security/person/";
            url += PERSON_ID;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
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

        //Autheticates a person
        [HttpGet]
        public AuthenticateModel PersonAuthenticateRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string USER_NAME, string PASSWORD)
        {
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/security/person/authenticate?";
            url += "applicationid=" + APPLICATION_ID + "&username=" + USER_NAME + "&password=" + PASSWORD;

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

            //serialize the response into an object
            AuthenticateModel person = JsonConvert.DeserializeObject<AuthenticateModel>(response);

            //return the object
            return person;
        }

        //updates a user role
        [HttpGet]
        public int UpdateRoleRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID, string ROLE)
        {
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/security/person/update/role?";
            url += "personid=" + PERSON_ID + "&role=" + ROLE;

            //Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Headers[HttpRequestHeader.Authorization] = authToken;
            request.ContentLength = 0;
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

            return 200;
        }

        //Password change request
        [HttpGet]
        public string PasswordChangeRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string USER_NAME)
        {
            string url = "https://service.proxomo.com/v09/json/security/person/passwordchange/request/";
            url += USER_NAME;
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

            //AppDataModel appData = JsonConvert.DeserializeObject<AppDataModel>(response);

            return response;

        }

        //Actually change the password
        [HttpGet]
        public PersonsModel PasswordChangeGetRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string USER_NAME, string PASSWORD, string PASSWORD_RESET_TOKEN)
        {
            string url = "https://service.proxomo.com/v09/json/security/person/passwordchange?";
            url += "username=" + USER_NAME + "&password=" + PASSWORD + "&resettoken=" + PASSWORD_RESET_TOKEN;
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

            //Create the web request
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

            PersonsModel appData = JsonConvert.DeserializeObject<PersonsModel>(response);

            return appData;

        }

        //Get a single person
        [HttpGet]
        public string PersonGet(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID)
        {
            string url = "https://service.proxomo.com/v09/json/person/";
            url += PERSON_ID;
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

            return response;
        }

        //Update a single Person
        [HttpPut]
        public string PersonUpdate(string APPLICATION_ID, string PROXOMO_API_KEY, string ID, string FullName)
        {
            string url = "https://service.proxomo.com/v09/json/person";
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
                    string json = "{\"ID\":\"" + ID + "\"," + "\"FullName\":\"" + FullName + "\"}";
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

        //Add app data to person
        [HttpPost]
        public string PersonAddDataAddRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID, string KEY, string VALUE, string OBJECT_TYPE)
        {
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/person/%PERSON_ID%/appdata";
            url = url.Replace("%PERSON_ID%", PERSON_ID);
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
                    string json = "{\"Key\":\"" + KEY + "\"," + "\"Value\":\"" + VALUE + "\",\"ObjectType\":\"" + OBJECT_TYPE + "\"}";
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

        //get app data for person
        [HttpGet]
        public AppDataModel PersonAppDataGetRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID, string APPDATA_ID)
        {
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string url = "https://service.proxomo.com/v09/json/person/%PERSON_ID%/appdata/%APPDATA_ID%";
            url = url.Replace("%PERSON_ID%", PERSON_ID).Replace("%APPDATA_ID%", APPDATA_ID);

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

            AppDataModel appData = JsonConvert.DeserializeObject<AppDataModel>(response);

            return appData;
            return appData;
        }

        //Get all appdata from person
        [HttpGet]
        public List<AppDataModel> PersonAppDataGetAll(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID)
        {
            string url = "https://service.proxomo.com/v09/json/person/%PERSON_ID%/appdata";
            url = url.Replace("%PERSON_ID%", PERSON_ID);
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

            List<AppDataModel> appData = JsonConvert.DeserializeObject<List<AppDataModel>>(response);

            return appData;
        }

        //Delete app data for a person
        [HttpDelete]
        public string PersonAppDataDelete(string APPLICATION_ID, string PROXOMO_API_KEY, string APPDATAAPP_ID, string APPDATAPERSON_ID)
        {
            string url = "https://service.proxomo.com/v09/json/person/%PERSON_ID%/appdata/%APPDATA_ID%";
            url = url.Replace("%PERSON_ID%", APPDATAPERSON_ID).Replace("%APPDATA_ID%", APPDATAAPP_ID);
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);

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

        //Update app data for a person
        [HttpPut]
        public string PersonAppDataUpdateRequest(string APPLICATION_ID, string PROXOMO_API_KEY, string PERSON_ID, string KEY, string VALUE, string OBJECT_TYPE)
        {
            string url = "https://service.proxomo.com/v09/json/person/%PERSON_ID%/appdata";
            url = url.Replace("%PERSON_ID%", PERSON_ID);
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
                    string json = "{\"Key\":\"" + KEY + "\"," + "\"Value\":\"" + VALUE + "\",\"ObjectType\":\"" + OBJECT_TYPE + "\"}";
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
