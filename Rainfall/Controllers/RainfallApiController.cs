using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Rainfall.Utilities;
using MediatR;
using Rainfall.Actions;

namespace Rainfall.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class RainfallApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        public RainfallApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <remarks></remarks>
        /// <param name="stationId">The id of the reading station</param>
        /// <param name="count">The number of readings to return</param>
        /// <response code="200">A list of rainfall readings successfully retrieved</response>
        /// <response code="400">Invalid request</response>
        [HttpGet]
        [Route("/api/v1/rainfall/id/{stationId}/readings")]
        [SwaggerOperation("GetRainfallReadings")]
        [SwaggerResponse(statusCode: 200, type: typeof(RainfallReadingResponse), description: "A list of rainfall readings successfully retrieved")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Invalid request")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] int count = 10)
        {
            if (string.IsNullOrEmpty(stationId))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request",
                    Detail = new List<ErrorDetail>
                    {
                        new ErrorDetail { PropertyName = "stationId", Message = "StationId cannot be null or empty" }
                    }
                });
            }

            if (count < 1 || count > 100)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request",
                    Detail = new List<ErrorDetail>
                    {
                        new ErrorDetail { PropertyName = "count", Message = "Count must be between 1 and 100" }
                    }
                });
            }

            var readings = await _mediator.Send(new GetRainfallReadingsReqest { Count = count, StationId = stationId });    
            return Ok(readings);
        }
    }
}
