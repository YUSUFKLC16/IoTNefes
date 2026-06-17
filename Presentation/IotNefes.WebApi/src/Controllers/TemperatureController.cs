using IotNefes.Abstractions.Temperature;
using IotNefes.Abstractions.Temperature.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class TemperatureController : GenericController<TemperatureDto, TemperatureRequest, TemperatureResponse>
    {
        public TemperatureController(
            ITemperatureService service,
            IApiMapper<TemperatureDto, TemperatureRequest, TemperatureResponse> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
