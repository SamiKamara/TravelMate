﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TravelMate.Services;

namespace TravelMate.Model
{
    public class ModelLogic
    {
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

        public static string ExtractInputFieldsData(UserSettingsService routeData)
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
