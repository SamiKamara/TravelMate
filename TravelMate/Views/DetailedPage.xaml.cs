using Microcharts;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Model;
using TravelMate.Services;
using TravelMate.ViewModels;
using SkiaSharp;

namespace TravelMate
{
    public partial class DetailedPage
    {
        private DetailedPageViewModel viewModel;
        public DetailedPage(DetailedPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            viewModel = vm;

            chartView.Chart = new RadialGaugeChart()
            {
                Entries = viewModel.entries,
                BackgroundColor = SKColors.Transparent,
                IsAnimated = false,
                LabelColor = SKColors.White,
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}