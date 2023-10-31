using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace TravelMate
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage()
        {
            InitializeComponent();
        }

        private async void OnGetRouteClicked(object sender, EventArgs e)
        {
            ResultEditor.Text = "";

            if (string.IsNullOrEmpty(StartCoords.Text) || string.IsNullOrEmpty(EndCoords.Text))
            {
                await DisplayAlert("Error", "Please enter both start and end street addresses.", "OK");
                return;
            }

            JObject startLocation = await GeocodingHelper.GetLocation(StartCoords.Text);
            if (startLocation["data"] == null || !startLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the start address.", "OK");
                return;
            }
            double startLat = startLocation["data"][0]["latitude"].Value<double>();
            double startLon = startLocation["data"][0]["longitude"].Value<double>();

            JObject endLocation = await GeocodingHelper.GetLocation(EndCoords.Text);
            if (endLocation["data"] == null || !endLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the end address.", "OK");
                return;
            }
            double endLat = endLocation["data"][0]["latitude"].Value<double>();
            double endLon = endLocation["data"][0]["longitude"].Value<double>();

            JObject endWeather = await WeatherHelper.GetWeather(endLat, endLon);
            string endWeatherData = ExtractWeatherData(endWeather.ToString());
            string inputWeatherData = ExtractInputFieldsData();

            double matchPercentage = CalculateMatchPercentage(endWeatherData, inputWeatherData);

            JObject route = await DigitransitHelper.GetRoute(startLat, startLon, endLat, endLon);

            ResultEditor.Text = $"Match Percentage between desired weather and end locations weather: {matchPercentage}%\n\n" + route.ToString();
        }

        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            ResultEditor.Text = "";

            if (string.IsNullOrEmpty(StreetAddressInput.Text))
            {
                await DisplayAlert("Error", "Please enter the street address.", "OK");
                return;
            }

            JObject location = await GeocodingHelper.GetLocation(StreetAddressInput.Text);

            // For demonstration purposes, display the raw JSON.
            ResultEditor.Text = location.ToString();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            ResultEditor.Text = "";

            if (string.IsNullOrEmpty(StreetAddressInput.Text))
            {
                await DisplayAlert("Error", "Please enter the street address.", "OK");
                return;
            }

            JObject location = await GeocodingHelper.GetLocation(StreetAddressInput.Text);
            if (location["data"] != null && location["data"].HasValues)
            {
                double lat = location["data"][0]["latitude"].Value<double>();
                double lon = location["data"][0]["longitude"].Value<double>();

                JObject weather = await WeatherHelper.GetWeather(lat, lon);

                string extractedWeatherInfo = ExtractWeatherData(weather.ToString());

                ResultEditor.Text = extractedWeatherInfo + "\n\n" + weather.ToString();
            }
            else
            {
                await DisplayAlert("Error", "Could not retrieve location data.", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public static string ExtractWeatherData(string jsonInput)
        {
            JObject jsonData = JObject.Parse(jsonInput);

            // Convert temperature from Kelvin to Celsius
            double tempInCelsius = jsonData["main"]["temp"].Value<double>() - 273.15;

            // Adjust the temperature scale so that 50 corresponds to 0°C
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
            double tempValue = double.TryParse(tempEntry.Text, out var tempResult) ? tempResult : 0;
            double rainValue = double.TryParse(rainEntry.Text, out var rainResult) ? rainResult : 0;
            double cloudsValue = double.TryParse(cloudsEntry.Text, out var cloudsResult) ? cloudsResult : 0;
            double windValue = double.TryParse(windEntry.Text, out var windResult) ? windResult : 0;

            string tempStr = FormatToThreeDigits(tempValue);
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
