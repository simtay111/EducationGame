using System;

namespace TangoApi.Entity
{
    public class TangoException : Exception
    {
        public TangoException(string message, string additional)
        {
            ErrorMessage = message;
            Additional = additional;
        }
        public string ErrorMessage { get; set; }
        public string Additional { get; set; }
    }
}