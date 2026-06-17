using IotNefes.Abstractions.Pm25;
using IotNefes.Abstractions.Pm25.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class Pm25Controller : GenericController<Pm25Dto, Pm25Request, Pm25Response>
    {
        public Pm25Controller(
            IPm25Service service,
            IApiMapper<Pm25Dto, Pm25Request, Pm25Response> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }
    }
}
