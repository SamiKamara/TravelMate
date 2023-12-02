using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Model;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public partial class ResultsPage : ContentPage
    {
        private ResultsPageViewModel viewModel;
        //public ResultsPage(ResultsPageViewModel vm)
        public ResultsPage(UserSettingsService routeSettings)
        {
            InitializeComponent();
            //BindingContext = vm;
            viewModel = new ResultsPageViewModel(routeSettings)
            {
                OnRouteSelected = NavigateToRouteDetails
            };

            BindingContext = viewModel;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void NavigateToRouteDetails(RouteModel routeModel)
        {
            await Navigation.PushAsync(new DetailedPage(routeModel));
        }
    }
}
