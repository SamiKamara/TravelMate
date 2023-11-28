using Microsoft.Maui.Controls;
using TravelMate.Services;

namespace TravelMate
{
    public partial class MainPage
    {
        private MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();

            viewModel = new MainPageViewModel();

            // Set the binding context to the MainPageViewModel instance
            this.BindingContext = viewModel;
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {
            if (await viewModel.ValidateAndNavigateAsync())
            {
                await Navigation.PushAsync(new WeatherPage(viewModel.RouteData));
            }
            else
            {
                await DisplayAlert("Error", "Invalid input or location data.", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            viewModel.LoadPreferences();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.SavePreferences();
        }
    }
}