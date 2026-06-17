using IotNefes.Abstractions.Co2;
using IotNefes.Abstractions.Co2.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class Co2Controller : GenericController<Co2Dto, Co2Request, Co2Response>
    {
        public Co2Controller(
            ICo2Service service,
            IApiMapper<Co2Dto, Co2Request, Co2Response> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
