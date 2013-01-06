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
            //Console.Write(Core.ExecuteStraigth("us", "ca", "Hollywood"));
            //Console.Write(Core.ExecuteStraigth("us", "90210"));
            //Console.Write(Core.GetNearbyStraigth("us", "90210"));

            Console.Write(Core.ExecuteStraigth(Countries.US, "ca", "Hollywood"));
            Console.Write(Core.ExecuteStraigth(Countries.US, "90210"));
            Console.Write(Core.GetNearbyStraigth(Countries.US, "90210"));

            Console.Read();
        }
    }
}
