using System.Collections.Generic;
using System.Linq;

namespace Domain.Statics
{
    public static class ProfanityFilter
    {
         public static bool IsValid(string stringToValidate)
         {
             var transformedString = stringToValidate.ToLower();
             var profanityList = new List<string>
                                     {
                                         "fuck",
                                         "damn",
                                         "nigger",
                                         "bitch",
                                         "shit",
                                     };

             return !profanityList.Any(transformedString.Contains);
         }
    }
}