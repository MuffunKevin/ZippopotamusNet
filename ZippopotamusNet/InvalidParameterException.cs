using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZippopotamusNet
{
    /// <summary>
    /// Define a proble with the provided search parameters.
    /// </summary>
    public class InvalidParameterException : Exception
    {
        private static string _errorMessage = "The provided parameter does not match any information";
 
        public InvalidParameterException() : base(_errorMessage) { }

        public InvalidParameterException(string message) : base(message) { }

        public InvalidParameterException(string message, Exception e) : base(message, e) { }

        public InvalidParameterException(Exception e) : base(_errorMessage, e) {}
    }
}
