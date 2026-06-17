using IotNefes.Abstractions.Pm10;
using IotNefes.Abstractions.Pm10.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class Pm10Controller : GenericController<Pm10Dto, Pm10Request, Pm10Response>
    {
        public Pm10Controller(
            IPm10Service service,
            IApiMapper<Pm10Dto, Pm10Request, Pm10Response> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
