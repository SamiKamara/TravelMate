using Microsoft.Maui.Controls;
using TravelMate.Services;

namespace TravelMate
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Create an instance of the UserSettingsService
            var userSettingsService = new UserSettingsService();

            // Set the binding context to the UserSettingsService instance
            this.BindingContext = userSettingsService;
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WeatherPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
