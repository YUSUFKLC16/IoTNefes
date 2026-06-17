using IotNefes.Abstractions.AirTemperature;
using IotNefes.Abstractions.AirTemperature.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Route("api/[controller]")]
    public class AirTemperatureController : GenericController<AirTemperatureDto, AirTemperatureRequest, AirTemperatureResponse>
    {
        public AirTemperatureController(
            IAirTemperatureService service,
            IApiMapper<AirTemperatureDto, AirTemperatureRequest, AirTemperatureResponse> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
