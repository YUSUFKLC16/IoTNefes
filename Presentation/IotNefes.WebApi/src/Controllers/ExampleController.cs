using IotNefes.Abstractions.Example;
using IotNefes.Abstractions.Example.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [Route("api/[controller]")]
    public class ExampleController : GenericController<ExampleDto, ExampleRequest, ExampleResponse>
    {
        public ExampleController(
            IExampleService service,
            IApiMapper<ExampleDto, ExampleRequest, ExampleResponse> mapper,
            IOutputCacheStore cacheStore)
            : base(service, mapper, cacheStore)
        {
        }

        [HttpGet("active")]
        [OutputCache(PolicyName = "CacheGet")]
        public async Task<IActionResult> GetActive()
        {
            var result = await Service.GetAllAsync();
            if (!result.IsSuccessful)
                return BadRequest(result.Error);

            var active = result.Data!.Where(x => x.IsActive).ToList();
            var response = Mapper.ToResponseList(active);
            return Ok(response);
        }
    }
}