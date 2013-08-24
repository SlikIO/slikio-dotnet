using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace SlikIO
{
    public class SlikIO
    {
        public string PrivateAPIKey { get; private set; }

        public string BaseUrl { get; set; }

        private WebClient client;

        public SlikIO(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey)) throw new Exception("You must specify a valid API key");

            PrivateAPIKey = privateKey;
            client = new WebClient();
            client.Credentials = new NetworkCredential(PrivateAPIKey, "");
            BaseUrl = "http://app.slik.io:3001/api/v1/";
        }

        public WebResponse sendData(string collectionID, Dictionary<string, object> data)
        {
            if (string.IsNullOrWhiteSpace(collectionID)) throw new Exception("You must specify a collection ID");

            string url = string.Format("collections/{0}/data", collectionID);
            return makePOSTRequest(url, data);
        }

        private WebResponse makePOSTRequest(string url, Dictionary<string, object> data)
        {
            string postData = createPostData(data);
            byte[] buffer = Encoding.ASCII.GetBytes(postData);

            WebRequest request = WebRequest.Create(BaseUrl + url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            request.Credentials = new NetworkCredential(PrivateAPIKey, "");
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            // Get the response.
            WebResponse response = request.GetResponse();
            return response;
        }


        private string createPostData(Dictionary<string, object> data)
        {
            string res = "";

            foreach (var item in data)
                res += "data[" + item.Key + "]=" + item.Value.ToString() + "&";

            return res.Remove(res.Length - 1, 1);
        }

    }
}
