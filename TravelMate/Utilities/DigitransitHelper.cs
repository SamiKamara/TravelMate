using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;

public static class DigitransitHelper
{
    private const string BaseUrl = "https://api.digitransit.fi/routing/v1/routers/finland/index/graphql";
    private const string SubscriptionKey = "235c8c6202c045078074f901deecd65f";
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task<JObject> GetRoute(double startLat, double startLng, double endLat, double endLng)
    {
        string query = $@"
    {{
        plan(from: {{lat: {startLat}, lon: {startLng}}}, 
             to: {{lat: {endLat}, lon: {endLng}}}, 
             numItineraries: 3) 
        {{
            itineraries 
            {{
                legs 
                {{
                    startTime
                    endTime
                    mode
                    from {{
                        name
                    }}
                    to {{
                        name
                    }}
                    duration
                    realTime
                    distance
                    transitLeg
                }}
            }}
        }}
    }}";

        var requestPayload = new StringContent(JsonConvert.SerializeObject(new { query = query }), Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Add("digitransit-subscription-key", SubscriptionKey);

        var response = await httpClient.PostAsync(BaseUrl, requestPayload);
        string jsonResponse = await response.Content.ReadAsStringAsync();

        return JObject.Parse(jsonResponse);
    }
}
