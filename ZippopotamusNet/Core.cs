using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

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
        /// <param name="country">The country.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraigth(Countries country, string zipcode)
        {
            return ExecuteStraigth(country.ToString(), zipcode);
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

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="state">The two letter ISO code of the state.</param>
        /// <param name="city">The city.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraigth(Countries country, string state, string city)
        {
            return ExecuteStraigth(country.ToString(), state, city);
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
            catch (WebException e)
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
        /// <param name="country">The two letter ISO country code.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string GetNearbyStraigth(string country, string zipcode)
        {
            return ExecuteStraigth("nearby", country, zipcode, string.Empty);
        }

        /// <summary>
        /// Gets the nearby place with the provided information the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string GetNearbyStraigth(Countries country, string zipcode)
        {
            return ExecuteStraigth("nearby", country.ToString(), zipcode, string.Empty);
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

        #region postal code
        public static ZipCodeInfo GetPostalCodeInfo(Countries contry, string zipCode)
        {
            var result = new ZipCodeInfo();

            var returnedJson = Core.ExecuteStraigth(contry, zipCode);
            var o = JObject.Parse(returnedJson);

            result = JsonConvert.DeserializeObject<ZipCodeInfo>(returnedJson);
            result.PostalCode = (string)o["post code"];
            result.CountryCode = (Countries)Enum.Parse(typeof(Countries), (string)o["country abbreviation"]);
            
            var places = (JArray)o["places"];
            
            for (var i = 0; i < places.Count; i++)
            {
                result.Places.Add(new Place());
                result.Places[i].PlaceName = (string)places[i]["place name"];
                result.Places[i].StateAbbreviation = (string)places[i]["state abbreviation"];
            }
            

            return result;
        }
        #endregion 
        
        #region places
        public static PlaceInformation GetPlacesInfo(Countries country, string stateCode, string city)
        {
            var result = new PlaceInformation();
            
            var returnedJson = Core.ExecuteStraigth(country, stateCode, city);
            var o = JObject.Parse(returnedJson);
            
            result = JsonConvert.DeserializeObject<PlaceInformation>(returnedJson);
            result.CountryCode = (Countries)Enum.Parse(typeof(Countries), (string)o["country abbreviation"]);
            result.PlaceName = (string)o["place name"];
            result.StateCode = (string)o["state abbreviation"];
            
            var places = (JArray)o["places"];
            
            for (var i = 0; i < places.Count; i++)
            {
                var newPlace       = new Place();
                newPlace.PlaceName = (string)places[i]["place name"];
                newPlace.Longitude = double.Parse((string)places[i]["longitude"], CultureInfo.InvariantCulture);
                newPlace.Latitude  = double.Parse((string)places[i]["latitude"], CultureInfo.InvariantCulture);
                newPlace.ZipCode   = (string)places[i]["post code"];
                result.Places.Add(newPlace);
            }
            
            return result;
        }

        public static NearbyInformation GetNearby(Countries country, string zipcode)
        {
            var result = new NearbyInformation();
            var returnedJson = Core.GetNearbyStraigth(country, zipcode);
            var o = JObject.Parse(returnedJson);

            result = JsonConvert.DeserializeObject<NearbyInformation>(returnedJson);
             
            result.NearLongitude = double.Parse((string)o["near longitude"], CultureInfo.InvariantCulture);
            result.NearLatitude = double.Parse((string)o["near latitude"], CultureInfo.InvariantCulture);

            var nearbies = (JArray)o["nearby"];

            for (var i = 0; i < nearbies.Count; i++)
            {
                var nearby       = new Nearby();
                nearby.Distance  = double.Parse((string)nearbies[i]["distance"], CultureInfo.InvariantCulture);
                nearby.PlaceName = (string)nearbies[i]["place name"];
                nearby.State     = (string)nearbies[i]["state"];
                nearby.StateCode = (string)nearbies[i]["state abbreviation"];
                nearby.ZipCode   = (string)nearbies[i]["postal code"];

                result.Nearbies.Add(nearby);
            }

            return result;
        }
        #endregion
    }
}
