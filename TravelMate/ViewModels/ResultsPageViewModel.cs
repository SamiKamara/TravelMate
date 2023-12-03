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


            JObject startLocation = await GeocodingHelper.GetLocation(routeData.From);
            if (startLocation["data"] == null || !startLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the start address.", "OK");
                return;
            }

            double startLat = startLocation["data"][0]["latitude"].Value<double>();
            double startLon = startLocation["data"][0]["longitude"].Value<double>();

            JObject endLocation = await GeocodingHelper.GetLocation(routeData.To);
            if (endLocation["data"] == null || !endLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the end address.", "OK");
                return;
            }

            double endLat = endLocation["data"][0]["latitude"].Value<double>();
            double endLon = endLocation["data"][0]["longitude"].Value<double>();
            string inputWeatherData = ExtractInputFieldsData();

            JObject route = await DigitransitHelper.GetRoute(startLat, startLon, endLat, endLon);

            var routeInfo = await GetCompactPublicTransportRoute(route.ToString(), inputWeatherData, endLat, endLon, routeData);

            var sortedRouteInfo = routeInfo.OrderByDescending(route => route.Date.Date == DateTime.Today).ThenBy(route => route.Date).ThenBy(route => route.StartTime).ToList();

            foreach (var routeModel in sortedRouteInfo)
            {
                routeModels.Add(routeModel);
            }

            IsBusy = false;
        }

        // This function demonstrates how to get the wanted data for routes
        // any non text form of output should draw inspiration from this
        // and make its own functions for similar purposes
        public static async Task<List<RouteModel>> GetCompactPublicTransportRoute(string json, string inputWeatherData, double endLat, double endLon, UserSettingsService routeData)
        {
            var jObject = JObject.Parse(json);
            var routes = new List<RouteModel>();

            foreach (var itinerary in jObject["data"]["plan"]["itineraries"])
            {
                var route = new RouteModel();

                DateTimeOffset? itineraryStartTime = null;
                DateTimeOffset? itineraryEndTime = null;

                foreach (var leg in itinerary["legs"])
                {
                    if (leg.Value<bool>("transitLeg"))
                    {
                        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(leg.Value<long>("startTime")).ToLocalTime();
                        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(leg.Value<long>("endTime")).ToLocalTime();
                        var duration = TimeSpan.FromSeconds(leg.Value<double>("duration"));

                        if (endTime < startTime)
                        {
                            endTime = endTime.AddDays(1);
                        }

                        itineraryStartTime ??= startTime;
                        itineraryEndTime = endTime;

                        var transportMode = new TransportMode
                        {
                            Mode = leg["mode"].ToString(),
                            StartTime = startTime.ToString("HH:mm"),
                            EndTime = endTime.ToString("HH:mm"),
                            Duration = duration.ToString("hh\\:mm"),
                            StartLocation = leg["from"]["name"].ToString(),
                            EndLocation = leg["to"]["name"].ToString()
                        };

                        route.TransportModes.Add(transportMode);
                    }
                }

                if (itineraryStartTime.HasValue && itineraryEndTime.HasValue)
                {
                    string arrivalTime = itineraryEndTime.Value.ToString("HH:mm");

                    // Get weather data for the end location at the arrival time
                    JObject forecastData = await WeatherHelper.GetWeatherForGivenTime(endLat, endLon, arrivalTime);
                    string endWeatherData = ExtractWeatherData(forecastData.ToString());
                    string[] weatherValues = endWeatherData.Split(',');

                    // Calculate the match percentage between the input data and the end weather data on arrival
                    double routeMatchPercentage = CalculateMatchPercentage(endWeatherData, inputWeatherData);
                    routeMatchPercentage = Math.Round(routeMatchPercentage, 1);

                    // Convert the end weather data to the same format as the input data and assign it to the route object
                    route.AssignWeatherData(weatherValues);

                    // Assign the input values, the weather match percentage, date and duration related values to the route object
                    route.CalculateTotalTravelTime(itineraryStartTime, itineraryEndTime);
                    route.StartTime = itineraryStartTime.Value.ToString("HH:mm");
                    route.ArrivalTime = arrivalTime;
                    route.From = routeData.From;
                    route.To = routeData.To;
                    route.Date = itineraryStartTime.Value.Date;
                    route.RouteMatchpercentage = routeMatchPercentage;
                    route.InputTemperature = routeData.Temperature;
                    route.InputRainChance = routeData.RainChance;
                    route.InputCloudiness = routeData.Cloudiness;
                    route.InputWindSpeed = routeData.WindSpeed;

                    routes.Add(route);
                }
            }
            return routes;
        }

        private Task DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

        public async Task GetLocationAsync(Editor resultEditor, string streetAddressInput)
        {
            resultEditor.Text = "";

            if (string.IsNullOrEmpty(streetAddressInput))
            {
                await DisplayAlert("Error", "Please enter the street address.", "OK");
                return;
            }

            JObject location = await GeocodingHelper.GetLocation(streetAddressInput);

            // For demonstration purposes, display the raw JSON.
            resultEditor.Text = location.ToString();
        }

        public async Task GetWeatherAsync(Editor resultEditor, string streetAddressInput)
        {
            resultEditor.Text = "";

            if (string.IsNullOrEmpty(streetAddressInput))
            {
                await DisplayAlert("Error", "Please enter the street address.", "OK");
                return;
            }

            JObject location = await GeocodingHelper.GetLocation(streetAddressInput);
            if (location["data"] != null && location["data"].HasValues)
            {
                double lat = location["data"][0]["latitude"].Value<double>();
                double lon = location["data"][0]["longitude"].Value<double>();

                JObject weather = await WeatherHelper.GetWeather(lat, lon);

                string extractedWeatherInfo = ExtractWeatherData(weather.ToString());

                resultEditor.Text = extractedWeatherInfo + "\n\n" + weather.ToString();
            }
            else
            {
                await DisplayAlert("Error", "Could not retrieve location data.", "OK");
            }
        }

        public static string ExtractWeatherData(string jsonInput)
        {
            JObject jsonData = JObject.Parse(jsonInput);

            // Convert temperature from Kelvin to Celsius
            double tempInCelsius = jsonData["main"]["temp"].Value<double>() - 273.15;

            // Adjust the temperature scale so that 50 corresponds to 0�C
            double adjustedTemp = 50 + tempInCelsius;

            // Check for rain in the description
            int chanceOfRain = jsonData["weather"][0]["description"].ToString().Contains("rain") ? 100 : 0;

            int cloudiness = jsonData["clouds"]["all"].Value<int>();

            double windSpeed = jsonData["wind"]["speed"].Value<double>();

            string tempStr = FormatToThreeDigits(adjustedTemp);
            string rainStr = FormatToThreeDigits(chanceOfRain);
            string cloudinessStr = FormatToThreeDigits(cloudiness);
            string windSpeedStr = FormatToThreeDigits(windSpeed);

            return $"{tempStr},{rainStr},{cloudinessStr},{windSpeedStr}";
        }

        public static string FormatToThreeDigits(double value)
        {
            int intValue = (int)Math.Round(value);
            intValue = Math.Clamp(intValue, 0, 100);
            return intValue.ToString("D3");
        }

        // For debugging, these will later come from sliders on weatherpage
        public string ExtractInputFieldsData()
        {
            double tempValue = double.TryParse(routeData.Temperature.ToString(), out var tempResult) ? tempResult : 0;
            double rainValue = double.TryParse(routeData.RainChance.ToString(), out var rainResult) ? rainResult : 0;
            double cloudsValue = double.TryParse(routeData.Cloudiness.ToString(), out var cloudsResult) ? cloudsResult : 0;
            double windValue = double.TryParse(routeData.WindSpeed.ToString(), out var windResult) ? windResult : 0;

            // same temperature offset as for destination weather
            string tempStr = FormatToThreeDigits(tempValue + 50);
            string rainStr = FormatToThreeDigits(rainValue);
            string cloudsStr = FormatToThreeDigits(cloudsValue);
            string windStr = FormatToThreeDigits(windValue);

            return $"{tempStr},{rainStr},{cloudsStr},{windStr}";
        }

        public static double CalculateMatchPercentage(string weatherData, string inputData)
        {
            string[] weatherValues = weatherData.Split(',');
            string[] inputValues = inputData.Split(',');

            if (weatherValues.Length != inputValues.Length) return 0;

            double totalPercentage = 0;

            for (int i = 0; i < weatherValues.Length; i++)
            {
                double weatherValue = double.Parse(weatherValues[i]);
                double inputValue = double.Parse(inputValues[i]);
                double maxVal = Math.Max(weatherValue, inputValue);
                double minVal = Math.Min(weatherValue, inputValue);
                double similarity = (maxVal == 0) ? 1 : minVal / maxVal;

                totalPercentage += similarity;
            }

            return (totalPercentage / weatherValues.Length) * 100;
        }

        private async Task OnResultsPageLoadedAsync()
        {
            try
            {
                await OnResultsPageLoaded();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task OnResultsPageLoaded()
        {
            await GetRouteAsync();
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out int result))
            {
                ((Entry)sender).Text = e.OldTextValue;
                return;
            }

            if (result < 0 || result > 100)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}
