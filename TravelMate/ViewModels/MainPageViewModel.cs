using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TravelMate.Services;

namespace TravelMate.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private UserSettingsService routeData;

        public MainPageViewModel(UserSettingsService param)
        {
            routeData = param;
        }

        public UserSettingsService RouteData
        {
            get { return routeData; }
            set
            {
                if (routeData != value)
                {
                    routeData = value;
                    OnPropertyChanged(nameof(RouteData));
                }
            }
        }

        public async Task<bool> ValidateAndNavigateAsync()
        {
            routeData.From = routeData.From.Trim();
            routeData.To = routeData.To.Trim();

            if (string.IsNullOrEmpty(routeData.From) || string.IsNullOrEmpty(routeData.To))
            {
                return false;
            }

            JObject startLocation = await GeocodingHelper.GetLocation(routeData.From);
            JObject endLocation = await GeocodingHelper.GetLocation(routeData.To);

            bool isValid = DataValidator.ValidateLocation(startLocation, endLocation);

            if (isValid)
            {
                RouteData.StartLocation = startLocation;
                RouteData.EndLocation = endLocation;
                return true;
            }

            return false;
        }

        public void SavePreferences()
        {
            Preferences.Set("FromLocation", RouteData.From);
            Preferences.Set("ToLocation", RouteData.To);
        }

        public void LoadPreferences()
        {
            string defaultFrom = string.Empty;
            string defaultTo = string.Empty;

            string storedFrom = Preferences.Get("FromLocation", defaultFrom);
            string storedTo = Preferences.Get("ToLocation", defaultTo);

            RouteData.From = !string.IsNullOrEmpty(storedFrom) ? storedFrom : defaultFrom;
            RouteData.To = !string.IsNullOrEmpty(storedTo) ? storedTo : defaultTo;
        }

        public Command NavigateToWeather =>
            new Command(ClickEvent);

        private async void ClickEvent(object obj)
        {
            if (await ValidateAndNavigateAsync())
            {
                dynamic viewModel = new WeatherPageViewModel(routeData);
                await App.Current.MainPage.Navigation.PushAsync(new WeatherPage(viewModel));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid input or location data.", "OK");
            }
        }
    }
}