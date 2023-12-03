using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelMate.Services
{
    public class DataValidator
    {
        public static bool ValidateLocation(JObject start, JObject end)
        {
            if (start["data"] == null || !start["data"].HasValues)
            {
                return false;
            }

            if (end["data"] == null || !end["data"].HasValues)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateRoute(JObject route)
        {
            if (route["data"]["plan"]["itineraries"].HasValues == false)
            {
                return false;
            }
            return true;
        }
    }
}
