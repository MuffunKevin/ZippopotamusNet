using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ZippopotamusNet
{
    public class Core
    {
        private static string _baseUrl = "http://api.zippopotam.us";

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The two letter ISO code of the country.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraigth(string country, string zipcode)
        {
            return ExecuteStraigth(country, string.Empty, string.Empty, zipcode);
        }

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The two letter ISO code of the country.</param>
        /// <param name="state">The two letter ISO code of the state.</param>
        /// <param name="city">The city.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraigth(string country, string state, string city)
        {
            return ExecuteStraigth(country, state, city, string.Empty);
        }

        private static string ExecuteStraigth(string country, string state, string city, string zipcode)
        {
            var result = string.Empty;

            ValidateIfTwoLetterCodeProvided(country, "country");
            ValidateIfTwoLetterCodeProvided(state, "state");

            country = FormatForUrl(country);
            state = FormatForUrl(state);
            city = FormatForUrl(city);
            zipcode = FormatForUrl(zipcode);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseUrl + country + state + city + zipcode);
            request.Method = "GET";
            request.ContentType = "application/json";

            try
            {
                using (var responseStream = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    result = responseStream.ReadToEnd();
                }
            }
            catch(WebException e)
            {
                if (e.Message.Contains("404"))
                {
                    throw new InvalidParameterException(e.Message);
                }
                else
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Validates if two letter code provided.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ZippopotamusNet.InvalidParameterException">The provided parameter ( + parameterName + ) was not a valid two letter ISO code</exception>
        private static void ValidateIfTwoLetterCodeProvided(string country, string parameterName)
        {
            if (!string.IsNullOrEmpty(country) && country.Length != 2 && country != "nearby")
            {
                throw new InvalidParameterException("The provided parameter (" + parameterName + ") was not a valid two letter ISO code");
            }
        }

        /// <summary>
        /// Gets the nearby place with the provided information the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string GetNearbyStraigth(string country, string zipcode)
        {
            return ExecuteStraigth("nearby", country, zipcode, string.Empty);
        }

        /// <summary>
        /// Formats for URL.
        /// </summary>
        /// <param name="urlPart">The URL part.</param>
        /// <returns></returns>
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
