using Rainfall.Models;

namespace Rainfall.Services
{
    public interface IRainfallService
    {
        Task<Response> GetRaifallReadings(string stationId);
    }
}
