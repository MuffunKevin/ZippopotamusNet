using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZippopotamusNet;

namespace DummyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write(Core.ExecuteStraight("us", "ca", "Hollywood"));
            //Console.Write(Core.ExecuteStraight("us", "90210"));
            //Console.Write(Core.GetNearbyStraigth("us", "90210"));

            //Console.Write(Core.ExecuteStraight(Countries.US, "ca", "Hollywood"));
            //Console.Write(Core.ExecuteStraight(Countries.US, "90210"));
            //Console.Write(Core.GetNearbyStraigth(Countries.US, "90210"));

            //Console.WriteLine(Core.GetPostalCodeInfo(Countries.US, "90210"));
            //Console.WriteLine(Core.GetPlacesInfo(Countries.US, "CA", "San Francisco"));
            //Console.WriteLine(Core.GetPlacesInfo(Countries.CA, "QC", "Sherbrooke"));

            //Console.WriteLine(Core.GetNearby(Countries.US, "90210"));
            Console.WriteLine(Core.GetNearby(Countries.CA, "J1H"));

            Console.Read();
        }
    }
}
