using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

public static class WeatherHelper
{
    private const string SubscriptionKey = "bebd592ae4675b54efe08a614156916c";
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task<JObject> GetWeather(double latitude, double longitude)
    {
        string BaseUrl = $@"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={SubscriptionKey}";
        var response = await httpClient.GetAsync(BaseUrl);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JObject.Parse(jsonResponse);
    }

    public static async Task<JObject> GetWeatherForGivenTime(double latitude, double longitude, string time)
    {
        TimeSpan roundedTime = RoundToNearestEvenHour(TimeSpan.Parse(time));

        // Determine if the forecast is for today or tomorrow
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        bool isToday = currentTime <= roundedTime;

        // Construct the URL for the 5 day / 3-hour forecast API
        // Paid subscription would allow for hourly casts, but this is best we get for free
        string BaseUrl = $@"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={SubscriptionKey}";
        var response = await httpClient.GetAsync(BaseUrl);
        string jsonResponse = await response.Content.ReadAsStringAsync();

        JObject forecastData = JObject.Parse(jsonResponse);
        return ExtractForecastForTime(forecastData, roundedTime, isToday);
    }

    private static TimeSpan RoundToNearestEvenHour(TimeSpan time)
    {
        int hours = time.Hours;
        int roundedHours = hours + (hours % 2 == 0 ? 0 : 1);
        return new TimeSpan(roundedHours, 0, 0);
    }

    private static JObject ExtractForecastForTime(JObject forecastData, TimeSpan roundedTime, bool isToday)
    {
        DateTime targetDate = DateTime.Today.Add(roundedTime);
        if (!isToday)
        {
            targetDate = targetDate.AddDays(1);
        }

        foreach (var item in forecastData["list"])
        {
            DateTime itemDateTime = DateTimeOffset.FromUnixTimeSeconds((long)item["dt"]).DateTime;

            if (itemDateTime.Date == targetDate.Date && Math.Abs((itemDateTime - targetDate).Hours) <= 1.5)
            {
                return (JObject)item;
            }
        }

        return null;
    }
}
