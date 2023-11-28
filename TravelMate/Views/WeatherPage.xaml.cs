using Microsoft.Maui.Controls;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{   
    public partial class WeatherPage
    {
        public WeatherPage(WeatherPageViewModel vm)
        {
            InitializeComponent();
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

