using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZippopotamusNet
{
    public class ZipCodeInfo
    {
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public Countries CountryCode { get; set; }
        public List<Place> Places { get; set; }
    }
}
