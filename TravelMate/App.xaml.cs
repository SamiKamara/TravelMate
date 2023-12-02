namespace TravelMate
{
    public partial class App : Application
    {
        // All view pages have a dependency injection
        // in their constructors like this
        public App(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(mainPage);
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 1024;
            const int newHeight = 576;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }
    }
}