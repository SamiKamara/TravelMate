namespace TravelMate;

public partial class ResultsPage : ContentPage
{
	public ResultsPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}