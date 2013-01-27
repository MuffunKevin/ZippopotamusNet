using System;
using System.Collections.Generic;

namespace ZippopotamusNet
{
    public class PlaceInformation
    {
        public Countries CountryCode { get; set; }
        public string Country {get; set;}
        public string PlaceName{get; set;}
        public string State {get; set;}
        public string StateCode {get; set;}
        public List<Place> Places { get; set; }
        
        public PlaceInformation ()
        {
            this.Places = new List<Place>();
        }
    }
}

