using Microsoft.Maui.Controls;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public partial class MainPage
    { 
        // All view pages inject viewmodels like this
        MainPageViewModel viewModel;
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();

            // Set the binding context to the MainPageViewModel instance
            BindingContext = vm;
            viewModel = vm;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            // Moving LoadPreferences and SavePreferences to vm would require use of maui community toolkit
            // or adding additional logic to viewmodelbase and adding viewpagebase to all view pages
            // so as an exception they are used here.
            viewModel.LoadPreferences();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.SavePreferences();
        }
    }
}