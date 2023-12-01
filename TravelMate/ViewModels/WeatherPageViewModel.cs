using TravelMate.ViewModels;
using TravelMate.Services;

namespace TravelMate
{
    public class WeatherPageViewModel : ViewModelBase
    {

        private UserSettingsService userSettings;

        public WeatherPageViewModel(UserSettingsService routeData)
        {
            userSettings = routeData;
        }

        public string From
        {
            get => userSettings.From;
        }

        public string To
        {
            get => userSettings.To;
        }

        public int Temperature
        {
            get => userSettings.Temperature;
            set
            {
                if (userSettings.Temperature != value)
                {
                    userSettings.Temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        public int RainChance
        {
            get => userSettings.RainChance;
            set
            {
                if (userSettings.RainChance != value)
                {
                    userSettings.RainChance = value;
                    OnPropertyChanged(nameof(RainChance));
                }
            }
        }

        public int Cloudiness
        {
            get => userSettings.Cloudiness;
            set
            {
                if (userSettings.Cloudiness != value)
                {
                    userSettings.Cloudiness = value;
                    OnPropertyChanged(nameof(Cloudiness));
                }
            }
        }

        public double WindSpeed
        {
            get => userSettings.WindSpeed;
            set
            {
                if (userSettings.WindSpeed != value)
                {
                    userSettings.WindSpeed = value;
                    OnPropertyChanged(nameof(WindSpeed));
                }
            }
        }

        public UserSettingsService RouteData
        {
            get { return userSettings; }
            set { userSettings = value; }
        }
    }
}