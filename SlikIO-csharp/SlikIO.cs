using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace SlikIO
{
    public class SlikIO
    {
        public string PrivateAPIKey { get; private set; }

        public string BaseUrl { get; private set; }

        public SlikIO(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey)) throw new Exception("You must specify a valid API key");

            PrivateAPIKey = privateKey;
            BaseUrl = "https://app.slik.io/api/v1/";
        }

        /// <summary>
        /// Sends data(dictionary) to a specified collection.
        /// </summary>
        /// <param name="collectionID">The collection ID you want to send the data to.</param>
        /// <param name="data">The data you want to send</param>
        public WebResponse SendData(string collectionID, Dictionary<string, object> data)
        {
            if (string.IsNullOrWhiteSpace(collectionID)) throw new Exception("You must specify a collection ID");

            string url = string.Format("collections/{0}/data", collectionID);
            return makePOSTRequest(url, data);
        }

        private WebResponse makePOSTRequest(string url, Dictionary<string, object> data)
        {
            string postData = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "data", data } });
            byte[] buffer = Encoding.ASCII.GetBytes(postData);

            WebRequest request = WebRequest.Create(BaseUrl + url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = buffer.Length;
            string auth = Convert.ToBase64String(Encoding.Default.GetBytes(PrivateAPIKey));
            request.Headers["Authorization"] = "Basic " + auth;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            // Get the response.
            WebResponse response = request.GetResponse();
            return response;
        }

    }
}
