using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZippopotamusNet
{
    public class NearbyInformation
    {
        public double NearLongitude { get; set; }
        public double NearLatitude { get; set; }
        public List<Nearby> Nearbies { get; set; }

        public NearbyInformation()
        {
            this.Nearbies = new List<Nearby>();
        }
    }
}
