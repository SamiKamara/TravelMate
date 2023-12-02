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