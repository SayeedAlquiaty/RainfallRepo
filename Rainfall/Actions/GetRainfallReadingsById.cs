using MediatR;
using Rainfall.Services;
using System.Diagnostics.Metrics;

namespace Rainfall.Actions
{
    // Request
    public class GetRainfallReadingsReqest : IRequest<RainfallReadingResponse>
    {
        public string StationId { get; set; }
        public int Count { get; set; }
    }

    // Response
    public class RainfallReadingResponse
    {
        public List<RainfallReading> Readings { get; set; }
    }

    public class RainfallReading
    {
        public DateTime DateMeasured { get; set; }
        public decimal AmountMeasured { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<ErrorDetail> Detail { get; set; }
    }

    public class ErrorDetail
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }

    public class GetProductByIdHandler : IRequestHandler<GetRainfallReadingsReqest, RainfallReadingResponse>
    {
        private readonly IRainfallService _rainfallService;
        public GetProductByIdHandler(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }
        public async Task<RainfallReadingResponse> Handle(GetRainfallReadingsReqest request, CancellationToken cancellationToken)
        {
            var result = await _rainfallService.GetRaifallReadings(request.StationId);

            var rainfallReadings = new List<RainfallReading>();

            foreach(var item in result?.Items.Take(request.Count))
            {
                rainfallReadings.Add(new RainfallReading { DateMeasured = item.DateTime, AmountMeasured = item.Value });
            }

            return new RainfallReadingResponse { Readings = rainfallReadings };
        }
    }
}
