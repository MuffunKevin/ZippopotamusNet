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
        public static string ExecuteStraight(string country, string zipcode)
        {
            return ExecuteStraight(country, string.Empty, string.Empty, zipcode);
        }

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraight(Countries country, string zipcode)
        {
            return ExecuteStraight(country.ToString(), zipcode);
        }

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The two letter ISO code of the country.</param>
        /// <param name="state">The two letter ISO code of the state.</param>
        /// <param name="city">The city.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraight(string country, string state, string city)
        {
            return ExecuteStraight(country, state, city, string.Empty);
        }

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="state">The two letter ISO code of the state.</param>
        /// <param name="city">The city.</param>
        /// <returns>The coplete received Json.</returns>
        public static string ExecuteStraight(Countries country, string state, string city)
        {
            return ExecuteStraight(country.ToString(), state, city);
        }

        /// <summary>
        /// Executes the straigth request to the API the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="state">The state.</param>
        /// <param name="city">The city.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        /// <exception cref="InvalidParameterException"></exception>
        private static string ExecuteStraight(string country, string state, string city, string zipcode)
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
            return ExecuteStraight("nearby", country, zipcode, string.Empty);
        }

        /// <summary>
        /// Gets the nearby place with the provided information the return will not be format or modify.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="zipcode">The zipcode.</param>
        /// <returns>The coplete received Json.</returns>
        public static string GetNearbyStraigth(Countries country, string zipcode)
        {
            return ExecuteStraight("nearby", country.ToString(), zipcode, string.Empty);
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

            var returnedJson = Core.ExecuteStraight(contry, zipCode);
            var o = JObject.Parse(returnedJson);

            result = JsonConvert.DeserializeObject<ZipCodeInfo>(returnedJson);
            result.PostalCode = o["post code"].ToString();
            result.CountryCode = (Countries)Enum.Parse(typeof(Countries), o["country abbreviation"].ToString());

            var places = o["places"] as JArray;

            for (var i = 0; i < places.Count; i++)
            {
                var place = new Place();
                
                place.PlaceName = places[i]["place name"].ToString();
                place.StateAbbreviation = places[i]["state abbreviation"].ToString();
                result.Places.Add(place);
            }

            return result;
        }
        #endregion

        #region places
        public static PlaceInformation GetPlacesInfo(Countries country, string stateCode, string city)
        {
            var result = new PlaceInformation();

            var returnedJson = Core.ExecuteStraight(country, stateCode, city);
            var o = JObject.Parse(returnedJson);

            result = JsonConvert.DeserializeObject<PlaceInformation>(returnedJson);
            result.CountryCode = (Countries)Enum.Parse(typeof(Countries), (string)o["country abbreviation"]);
            result.PlaceName = o["place name"].ToString();
            result.StateCode = o["state abbreviation"].ToString();

            var places = o["places"] as JArray;

            for (var i = 0; i < places.Count; i++)
            {
                var newPlace = new Place();

                newPlace.PlaceName = places[i]["place name"].ToString();
                newPlace.Longitude = double.Parse(places[i]["longitude"].ToString(), CultureInfo.InvariantCulture);
                newPlace.Latitude = double.Parse(places[i]["latitude"].ToString(), CultureInfo.InvariantCulture);
                newPlace.ZipCode = places[i]["post code"].ToString();

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

            result.NearLongitude = double.Parse(o["near longitude"].ToString(), CultureInfo.InvariantCulture);
            result.NearLatitude = double.Parse(o["near latitude"].ToString(), CultureInfo.InvariantCulture);

            var nearbies = o["nearby"] as JArray;

            for (var i = 0; i < nearbies.Count; i++)
            {
                var nearby = new Nearby();

                nearby.Distance = double.Parse(nearbies[i]["distance"].ToString(), CultureInfo.InvariantCulture);
                nearby.PlaceName = nearbies[i]["place name"].ToString();
                nearby.State = nearbies[i]["state"].ToString();
                nearby.StateCode = nearbies[i]["state abbreviation"].ToString();
                nearby.ZipCode = nearbies[i]["postal code"].ToString();

                result.Nearbies.Add(nearby);
            }
            
            return result;
        }
        #endregion
    }
}
