using IotNefes.Abstractions.Humidity;
using IotNefes.Abstractions.Humidity.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class HumidityController : GenericController<HumidityDto, HumidityRequest, HumidityResponse>
    {
        public HumidityController(
            IHumidityService service,
            IApiMapper<HumidityDto, HumidityRequest, HumidityResponse> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
