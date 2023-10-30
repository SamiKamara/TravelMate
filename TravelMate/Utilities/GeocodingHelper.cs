using Newtonsoft.Json.Linq;

public static class GeocodingHelper
{
    private const string SubscriptionKey = "77e02259e5d2f825d9329a3cbd705c7f";
    private static readonly HttpClient HttpClient = new HttpClient();

    public static async Task<JObject> GetLocation(string cityName)
    {
        string parameter = $@"http://api.positionstack.com/v1/forward?access_key={SubscriptionKey}&query={cityName}";

        var response = await HttpClient.GetAsync(parameter);
        string jsonResponse = await response.Content.ReadAsStringAsync();

        return JObject.Parse(jsonResponse);
    }
}