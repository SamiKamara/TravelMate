using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelMate.Services
{
    public class UserSettingsService
    {
        private string from;
        private string to;
        private int temperature;
        private int rainChance;
        private int cloudiness;
        private double windSpeed;

        
        public string From
        {
            get { return from; }
            set
            {
                if (from != value)
                {
                    LogUserSettingsChange("From", from, value);
                    from = value;
                }
            }
        }

        public string To
        {
            get { return to; }
            set
            {
                if (to != value)
                {
                    LogUserSettingsChange("To", to, value);
                    to = value;
                }
            }
        }

        public int Temperature
        {
            get { return temperature; }
            set
            {
                if (temperature != value)
                {
                    LogUserSettingsChange("Temperature", temperature.ToString(), value.ToString());
                    temperature = value;
                }
            }
        }

        public int RainChance
        {
            get { return rainChance; }
            set
            {
                if (rainChance != value)
                {
                    LogUserSettingsChange("RainChance", rainChance.ToString(), value.ToString());
                    rainChance = value;
                }
            }
        }

        public int Cloudiness
        {
            get { return cloudiness; }
            set
            {
                if (cloudiness != value)
                {
                    LogUserSettingsChange("Cloudiness", cloudiness.ToString(), value.ToString());
                    cloudiness = value;
                }
            }
        }

        public double WindSpeed
        {
            get { return windSpeed; }
            set
            {
                if (windSpeed != value)
                {
                    LogUserSettingsChange("WindSpeed", windSpeed.ToString(), value.ToString());
                    windSpeed = value;
                }
            }
        }

        public UserSettingsService()
        {
            // Initialize the properties if needed
            from = "";
            to = "";
        }

        // Log changes to the console
        private void LogUserSettingsChange(string propertyName, string oldValue, string newValue)
        {
            System.Diagnostics.Debug.WriteLine($"User settings change: {propertyName} changed from '{oldValue}' to '{newValue}'");
        }
    }
}