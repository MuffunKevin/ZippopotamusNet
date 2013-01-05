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
            Console.Write(Core.ExecuteStraith("us", string.Empty, string.Empty, "90210"));
            Console.Read();
        }
    }
}
