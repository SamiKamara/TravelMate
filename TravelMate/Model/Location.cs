namespace TravelMate.Model
{
    public class AddressLocation
    {
        public string Address { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        private readonly Dictionary<string, double> _weather;

        public AddressLocation(string address, double latitude, double longitude)
        {
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            _weather = new Dictionary<string, double>();

            obtainWeather();
        }

        private async void obtainWeather()
        {
            dynamic weather = await WeatherHelper.GetWeather(Latitude, Longitude);

            double temp = weather.main.temp;
            double cloudiness = weather.clouds.all;
            double windspeed = weather.wind.speed;
            double rain;

            if (weather.ContainsKey("rain"))
            {
                rain = weather.rain["1h"];
            }
            else
            {
                rain = 0.0;
            }

            _weather.Add("temp", temp);
            _weather.Add("cloudiness", cloudiness);
            _weather.Add("windspeed", windspeed);
            _weather.Add("rain", rain);
        }
    }
}
