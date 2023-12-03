using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microcharts;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using TravelMate.Model;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public class DetailedPageViewModel : ViewModelBase
    {
        private RouteModel routeModel;

        public DetailedPageViewModel(RouteModel route)
        {
            routeModel = route;
            BackClickCommand = new Command(Back);
            InitializeChartEntries();
        }

        public Command BackClickCommand { get; set; }

        public string FromStr => $"From: {routeModel.From} ({routeModel.StartTime})";
        public string ToStr => $"To: {routeModel.To} ({routeModel.ArrivalTime})";
        public string RouteMatchStr => $"Route match: {routeModel.RouteMatchpercentage}%";
        public string FormattedTotalTravelTimeStr => $"Total travel time: {routeModel.FormattedTotalTravelTime}h";
        public string TemperatureMatchStr => $"Temperate match: wanted: {routeModel.InputTemperature}c, got: {routeModel.ResultTemperature}c";
        public string WeatherMatchStr => $"Rain match: wanted: {routeModel.InputRainChance}%, got: {routeModel.ResultRainChance}%";
        public string CloudinessMatchStr => $"Cloudiness match: wanted: {routeModel.InputCloudiness}%, got: {routeModel.ResultCloudiness}%";
        public string WindSpeedMatchStr => $"Wind speed match: wanted: {routeModel.InputWindSpeed}m/s, got: {routeModel.ResultWindSpeed}m/s";

        public string TransportModesStr
        {
            get
            {
                string result = "";
                foreach (TransportMode mode in routeModel.TransportModes)
                {
                    result += $"{mode.Mode} from {mode.StartLocation} to {mode.EndLocation} ({mode.StartTime} - {mode.EndTime}), duration {mode.Duration}\n";
                }
                return result;
            }
        }

        public ChartEntry[] entries;

        private async void Back()
        {
            try
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void InitializeChartEntries()
        {
            entries = new[]
            {
                new ChartEntry((float)routeModel.TemperatureMatchPercentage)
                {
                    Label = "Temperature match",
                    ValueLabel = (routeModel.TemperatureMatchPercentage).ToString() + "%",
                    Color = SKColor.Parse("#f1b44c"),
                    ValueLabelColor = SKColors.White
                },
                new ChartEntry((float)routeModel.RainChanceMatchPercentage)
                {
                    Label = "Rain match",
                    ValueLabel = (routeModel.RainChanceMatchPercentage).ToString() + "%",
                    Color = SKColor.Parse("#34c38f"),
                    ValueLabelColor = SKColors.White
                },
                new ChartEntry((float)routeModel.CloudinessMatchPercentage)
                {
                    Label = "Cloudiness match",
                    ValueLabel = (routeModel.CloudinessMatchPercentage).ToString() + "%",
                    Color = SKColor.Parse("#556ee6"),
                    ValueLabelColor = SKColors.White
                },
                new ChartEntry((float)routeModel.WindSpeedMatchPercentage)
                {
                    Label = "Wind speed match",
                    ValueLabel = (routeModel.WindSpeedMatchPercentage).ToString() + "%",
                    Color = SKColor.Parse("#e83e8c"),
                    ValueLabelColor = SKColors.White
                }
            };
        }
    }
}
