using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace TravelMate.ViewModels
{
    public class ResultsPageViewModel : ViewModelBase
    {
        private UserSettingsService routeData;

        public ResultsPageViewModel(UserSettingsService routeSettings)
        {
            routeData = routeSettings;
        }

        public async Task GetRouteAsync(Editor resultEditor, Label desiredWeather, Label destinationWeather)
        {
            resultEditor.Text = "";

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

            JObject endWeather = await WeatherHelper.GetWeather(endLat, endLon);
            string endWeatherData = ExtractWeatherData(endWeather.ToString());
            string inputWeatherData = ExtractInputFieldsData();

            double matchPercentage = CalculateMatchPercentage(endWeatherData, inputWeatherData);

            JObject route = await DigitransitHelper.GetRoute(startLat, startLon, endLat, endLon);

            string[] weatherValues = endWeatherData.Split(',');
            string[] inputValues = inputWeatherData.Split(',');

            desiredWeather.Text = $"Desired destination weather: temperature; {inputValues[0]} rain; {inputValues[1]} cloudiness; {inputValues[2]} windspeed; {inputValues[3]}";
            destinationWeather.Text = $"Destination weather: temperature; {weatherValues[0]} rain; {weatherValues[1]} cloudiness; {weatherValues[2]} windspeed; {weatherValues[3]}";

            resultEditor.Text = $"Match Percentage between desired weather and end locations weather: {matchPercentage}%\n\n" + route.ToString();
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
    }
}