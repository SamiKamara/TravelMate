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
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate.ViewModels;

public class WeatherPageViewModel : ViewModelBase
{
    private UserSettingsService routeData;
    public WeatherPageViewModel(UserSettingsService param)
    {
        routeData = param;
        BackClickCommand = new Command(Back);
        NextClickCommand = new Command(Forward);
    }

    public Command BackClickCommand { get; set; }

    public Command NextClickCommand { get; set; }

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

    private async void Forward()
        {
            try
            {
                dynamic viewModel = new ResultsPageViewModel(routeData);
                await App.Current.MainPage.Navigation.PushAsync(new ResultsPage(viewModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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
}