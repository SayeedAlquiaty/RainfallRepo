
using LazyCache;
using Rainfall.Models;
using Rainfall.Utilities;
using System.Text.Json;

namespace Rainfall.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly IAppCache _cache;

        public RainfallService(IAppCache cache)
        {
            _cache = cache;
        }

        public async Task<Response> GetRaifallReadings(string stationId)
        {
            // Define the API endpoint
            string url = "https://environment.data.gov.uk/flood-monitoring/id/stations/" + stationId + "/readings?_sorted&_limit=100";

            Func<Task<Response>> rainfallResponseFactory = () => PopulateShowsCache(url);
            var retVal = await _cache.GetOrAddAsync("Rainfall"+ stationId, rainfallResponseFactory, DateTimeOffset.Now.AddMinutes(15));
            return retVal;
        }

        private async Task<Response> PopulateShowsCache(string url)
        {
            var response = await HttpClientHelper.GetAsync(url);
            var responseJson = await response.Content.ReadFromJsonAsync<Response>().ConfigureAwait(false);
            
            return responseJson;
        }
    }
}

