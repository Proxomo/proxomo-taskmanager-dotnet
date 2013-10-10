using Newtonsoft.Json;
using ProxomoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProxomoApp.Controllers
{
    public class FileStorageController : ApiController
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


        public string PostingAFile(string APPLICATION_ID, string PROXOMO_API_KEY)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("id", "TTR");
            nvc.Add("btn-submit-photo", "Upload");


            string url = "http://storage.proxomo.com/api/Files";
            string response = string.Empty;
            string authToken = getAuthToken(APPLICATION_ID, PROXOMO_API_KEY);
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers[HttpRequestHeader.Authorization] = authToken;

            Stream rs = request.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }

            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "file", "C:\test\test.jpg", "image/jpeg");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream("C:\test\test.jpg", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = request.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                request = null;
            }
            return response;
        }
    }
}
