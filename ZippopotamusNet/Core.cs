using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ZippopotamusNet
{
    //TODOS:
    //- Make country a enum
    //- Make overload of the ExecuteStraith function
    //- Creaate class for each possible call of the api
    public class Core
    {
        private static string _baseUrl = "http://api.hippopotam.us";

        public static string ExecuteStraith(string country, string state, string city, string zipcode)
        {
            var result = string.Empty;

            country = FormatForUrl(country);
            state = FormatForUrl(state);
            city = FormatForUrl(city);
            zipcode = FormatForUrl(zipcode);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseUrl + country + state + city + zipcode);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (var responseStream = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                result = responseStream.ReadToEnd();
            }

            return result;
        }

        private static string FormatForUrl(string urlPart)
        {
            if (!string.IsNullOrEmpty(urlPart))
            {
                return "/" + urlPart;
            }
            return urlPart;
        }
    }
}
