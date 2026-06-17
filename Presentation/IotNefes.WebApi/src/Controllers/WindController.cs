using IotNefes.Abstractions.Wind;
using IotNefes.Abstractions.Wind.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class WindController : GenericController<WindDto, WindRequest, WindResponse>
    {
        public WindController(
            IWindService service,
            IApiMapper<WindDto, WindRequest, WindResponse> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
