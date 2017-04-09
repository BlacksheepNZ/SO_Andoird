using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Soical_Occasion
{
    public class API_Response
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseData { get; set; }
    }

    public class Login_Request
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Register_Request
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public static class Http
    {
        public static String Post(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, pairs);
            }
            return Encoding.Default.GetString(response);
        }
    }
}