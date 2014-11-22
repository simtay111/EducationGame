using System.Text.RegularExpressions;

namespace Domain
{
    public static class RegexLibrary
    {
         public static bool MatchForEmail(string stringToValidate)
         {
             var regex = new Regex(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$");

             return regex.IsMatch(stringToValidate);
         }
    }
}