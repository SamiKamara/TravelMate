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

        public RouteModel RouteModel
        {
            get => routeModel;
            set
            {
                if (routeModel != value)
                {
                    routeModel = value;
                    OnPropertyChanged(nameof(RouteModel));
                }
            }
        }

        public DetailedPageViewModel(RouteModel route)
        {
            routeModel = route;
            BackClickCommand = new Command(Back);
        }
        public Command BackClickCommand { get; set; }

        public string FromStr => $"From: {routeModel.From} ({routeModel.StartTime})";
        public string ToStr => $"To: {routeModel.To} ({routeModel.ArrivalTime})";
        public string RouteMatchStr => $"Route match: {routeModel.RouteMatchpercentage}%";
        public string FormattedTotalTravelTimeStr => $"Total travel time: {routeModel.FormattedTotalTravelTime}h";
        public string TemperatureMatchStr => $"Temperate match: wanted: {routeModel.InputTemperature}%, got: {routeModel.ResultTemperature}%";
        public string WeatherMatchStr => $"Rain match: wanted: {routeModel.InputRainChance}%, got: {routeModel.ResultRainChance}%";
        public string CloudinessMatchStr => $"Cloudiness match: wanted: {routeModel.InputCloudiness}%, got: {routeModel.ResultCloudiness}%";
        public string WindSpeedMatchStr => $"Wind speed match: wanted: {routeModel.InputWindSpeed}m/s, got: {routeModel.ResultWindSpeed}m/s";


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

        public ChartEntry[] entries = new[]
        {
            new ChartEntry(212)
            {
                Label = "Test1",
                ValueLabel = "212",
                Color = SKColor.Parse("#44a5ff"),
                ValueLabelColor = SKColors.White
            },
            new ChartEntry(248)
            {
                Label = "Test2",
                ValueLabel = "248",
                Color = SKColor.Parse("#44a5ff"),
                ValueLabelColor = SKColors.White
            },
            new ChartEntry(129)
            {
                Label = "Test3",
                ValueLabel = "129",
                Color = SKColor.Parse("#44a5ff"),
                ValueLabelColor = SKColors.White
            },
            new ChartEntry(69)
            {
                Label = "Test4",
                ValueLabel = "69",
                Color = SKColor.Parse("#44a5ff"),
                ValueLabelColor = SKColors.White
            }
        };
    }
}