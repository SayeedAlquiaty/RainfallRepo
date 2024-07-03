
using Rainfall.Models;
using Rainfall.Utilities;
using System.Text.Json;

namespace Rainfall.Services
{
    public class RainfallService : IRainfallService
    {
        public async Task<Response> GetRaifallReadings(string stationId, int count)
        {
            // Define the API endpoint
            string url = "https://environment.data.gov.uk/flood-monitoring/id/stations/" + stationId + "/readings?_sorted&_limit=" + count;

            var responseString =  await HttpClientHelper.GetAsync(url);

            var obj = JsonSerializer.Deserialize<object>(responseString);
            var response = JsonSerializer.Deserialize<Response>(obj.ToString());

            return (Response)response;
        }
    }
}
