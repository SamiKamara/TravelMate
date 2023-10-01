using Microsoft.Maui.Controls;

namespace TravelMate
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WeatherPage());
            //all kinds of stuff related to getting the routes should be added here
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
