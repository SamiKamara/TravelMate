using Microsoft.Maui.Controls;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{   
    public partial class WeatherPage
    {
        private WeatherPageViewModel viewModel;
        public WeatherPage(UserSettingsService routeSettings)
        public WeatherPage(WeatherPageViewModel vm)
        {
            InitializeComponent();

            viewModel = new WeatherPageViewModel(routeSettings);

            BindingContext = viewModel;
            
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}

