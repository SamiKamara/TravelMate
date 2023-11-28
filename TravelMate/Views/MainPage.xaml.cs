using Microsoft.Maui.Controls;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public partial class MainPage
    { 
        // All view pages inject viewmodels like this
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();

            // Set the binding context to the MainPageViewModel instance
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}