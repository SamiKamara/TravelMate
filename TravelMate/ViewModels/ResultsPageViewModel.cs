using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Text;
using System.Collections.ObjectModel;
using TravelMate.Model;
using System.Linq;
using System.Windows.Input;

namespace TravelMate.ViewModels
{
    public class ResultsPageViewModel : ViewModelBase
    {
        private UserSettingsService routeData;
        public ObservableCollection<RouteModel> routeModels { get; set; }
        public ICommand SelectRouteCommand { get; }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        private bool isDataLoaded;
        public bool IsDataLoaded
        {
            get => isDataLoaded;
            set
            {
                if (isDataLoaded != value)
                {
                    isDataLoaded = value;
                    OnPropertyChanged(nameof(IsDataLoaded));
                }
            }
        }

        public ResultsPageViewModel(UserSettingsService routeSettings)
        {
            routeData = routeSettings;
            _ = OnResultsPageLoadedAsync();
            routeModels = new ObservableCollection<RouteModel>();
            BackClickCommand = new Command(Back);
            SelectRouteCommand = new Command<RouteModel>(ExecuteSelectedRoute);
        }
        public Command BackClickCommand { get; set; }

        private async void Back()
        {
            try
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void ExecuteSelectedRoute(RouteModel selectedRoute)
        {
            DetailedPageViewModel viewModel = new DetailedPageViewModel(selectedRoute);
            await App.Current.MainPage.Navigation.PushAsync(new DetailedPage(viewModel));
        }

        public Action<RouteModel> OnRouteSelected { get; set; }

        public async Task GetRouteAsync()
        {
            IsBusy = true;

            double startLat = routeData.StartLocation["data"][0]["latitude"].Value<double>();
            double startLon = routeData.StartLocation["data"][0]["longitude"].Value<double>();

            double endLat = routeData.EndLocation["data"][0]["latitude"].Value<double>();
            double endLon = routeData.EndLocation["data"][0]["longitude"].Value<double>();

            string inputWeatherData = ModelLogic.ExtractInputFieldsData(routeData);

            JObject route = await DigitransitHelper.GetRoute(startLat, startLon, endLat, endLon);

            bool isValid = DataValidator.ValidateRoute(route);

            if (isValid)
            {
                //var routeInfo = await GetCompactPublicTransportRoute(route.ToString(), inputWeatherData, endLat, endLon, routeData);

                var routeInfo = await ModelLogic.GetCompactPublicTransportRoute(route.ToString(), inputWeatherData, endLat, endLon, routeData);

                var sortedRouteInfo = routeInfo.OrderByDescending(route => route.Date.Date == DateTime.Today).ThenBy(route => route.Date).ThenBy(route => route.StartTime).ToList();

                foreach (var routeModel in sortedRouteInfo)
                {
                    routeModels.Add(routeModel);
                }

            } else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Could not retrieve route data for given addresses, please return and change them.", "OK");
            }

            IsBusy = false;
        }

        private async Task OnResultsPageLoadedAsync()
        {
            try
            {
                await OnResultsPageLoaded();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task OnResultsPageLoaded()
        {
            await GetRouteAsync();
        }
    }
}
