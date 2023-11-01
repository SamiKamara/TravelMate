using Microsoft.Maui.Controls;
using TravelMate.Services;

namespace TravelMate
{
    public partial class MainPage
    {
        private UserSettingsService userSettingsService;
        public MainPage()
        {
            InitializeComponent();

            // Create an instance of the UserSettingsService
            userSettingsService = new UserSettingsService();

            // Set the binding context to the UserSettingsService instance
            this.BindingContext = userSettingsService;
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new WeatherPage(userSettingsService));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
