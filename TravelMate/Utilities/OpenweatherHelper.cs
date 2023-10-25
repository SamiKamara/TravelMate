using Newtonsoft.Json.Linq;
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
}
