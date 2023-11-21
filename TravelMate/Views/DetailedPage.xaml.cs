using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
	public partial class DetailedPage
	{
		private DetailedPageViewModel viewModel;

		public DetailedPage()
		{
			InitializeComponent();

			viewModel = new DetailedPageViewModel();

			this.BindingContext = viewModel;
		}
	}
}