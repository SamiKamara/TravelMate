using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace TravelMate.Services
{
    public class UserSettingsService : INotifyPropertyChanged
    {
        private string from;
        private string to;
        private int temperature;
        private int rainChance;
        private int cloudiness;
        private double windSpeed;
        private JObject startLocation;
        private JObject endLocation;

        public event PropertyChangedEventHandler PropertyChanged;

        public string From
        {
            get { return from; }
            set
            {
                if (from != value)
                {
                    from = value;
                    OnPropertyChanged(nameof(From));
                    LogUserSettingsChange("From", from, value);
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
                    to = value;
                    OnPropertyChanged(nameof(To));
                    LogUserSettingsChange("To", to, value);
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
                    temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                    LogUserSettingsChange("Temperature", temperature.ToString(), value.ToString());
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
                    rainChance = value;
                    OnPropertyChanged(nameof(RainChance));
                    LogUserSettingsChange("RainChance", rainChance.ToString(), value.ToString());
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
                    cloudiness = value;
                    OnPropertyChanged(nameof(Cloudiness));
                    LogUserSettingsChange("Cloudiness", cloudiness.ToString(), value.ToString());
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
                    value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                    windSpeed = value;
                    OnPropertyChanged(nameof(WindSpeed));
                    LogUserSettingsChange("WindSpeed", windSpeed.ToString(), value.ToString());
                }
            }
        }

        public UserSettingsService()
        {
            from = "";
            to = "";
            temperature = 0;
            rainChance = 0;
            cloudiness = 0;
            windSpeed = 0.0;
        }

        public JObject StartLocation
        {
            get { return startLocation; }
            set
            {
                if (startLocation != value)
                {
                    startLocation = value;
                }
            }
        }

        public JObject EndLocation
        {
            get { return endLocation;}
            set
            {
                if (endLocation != value)
                {
                    endLocation = value;
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LogUserSettingsChange(string propertyName, string oldValue, string newValue)
        {
            Debug.WriteLine($"User settings change: {propertyName} changed from '{oldValue}' to '{newValue}'");
        }
    }
}
