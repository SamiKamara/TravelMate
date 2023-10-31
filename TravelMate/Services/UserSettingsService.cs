using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelMate.Services
{
    class UserSettingsService
    {
        private string from;
        private string to;

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