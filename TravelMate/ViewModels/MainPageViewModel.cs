using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public class MainPageViewModel : ViewModelBase
    {
        private UserSettingsService routeData;

        public MainPageViewModel()
        {
            routeData = new UserSettingsService();
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
            if (startLocation["data"] == null || !startLocation["data"].HasValues)
            {
                return false;
            }

            JObject endLocation = await GeocodingHelper.GetLocation(routeData.To);
            if (endLocation["data"] == null || !endLocation["data"].HasValues)
            {
                return false;
            }

            return true;
        }
    }
}