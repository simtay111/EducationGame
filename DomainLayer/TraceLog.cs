using System.Diagnostics;

namespace DomainLayer
{
    public static class TraceLog
    {
         public static void WriteLine(string message)
         {
             Trace.WriteLine(message);
         }
    }
}