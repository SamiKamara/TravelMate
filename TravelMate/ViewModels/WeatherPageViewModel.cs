using System.Diagnostics;
using TravelMate.Services;

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
        //dynamic viewModel = new ResultsPageViewModel(routeData);
        //await App.Current.MainPage.Navigation.PushAsync(new ResultsPage(viewModel));
        await App.Current.MainPage.Navigation.PushAsync(new ResultsPage(routeData));
    }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    } 
    public string From
    {
        get => routeData.From;
    }

    public string To
    {
        get => routeData.To;
    }

    public int Temperature
    {
        get => routeData.Temperature;
        set
        {
            if (routeData.Temperature != value)
            {
                routeData.Temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }
    }

    public int RainChance
    {
        get => routeData.RainChance;
        set
        {
            if (routeData.RainChance != value)
            {
                routeData.RainChance = value;
                OnPropertyChanged(nameof(RainChance));
            }
        }
    }

    public int Cloudiness
    {
        get => routeData.Cloudiness;
        set
        {
            if (routeData.Cloudiness != value)
            {
                routeData.Cloudiness = value;
                OnPropertyChanged(nameof(Cloudiness));
            }
        }
    }

    public double WindSpeed
    {
        get => routeData.WindSpeed;
        set
        {
            if (routeData.WindSpeed != value)
            {
                routeData.WindSpeed = value;
                OnPropertyChanged(nameof(WindSpeed));
            }
        }
    }

    public UserSettingsService RouteData
    {
        get { return routeData; }
        set { routeData = value; }
    }
}


 /*ublic UserSettingsService RouteData
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
    }*/
