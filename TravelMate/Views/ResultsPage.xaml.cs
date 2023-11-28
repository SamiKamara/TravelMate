using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(ResultsPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
