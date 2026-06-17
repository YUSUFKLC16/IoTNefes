using IotNefes.Abstractions;
using IotNefes.Common;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace IotNefes.WebApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ETagFilter))]
    public abstract class GenericController<TDto, TRequest, TResponse> : ControllerBase
        where TDto : class
        where TRequest : class
        where TResponse : class
    {
        protected readonly IGenericService<TDto> Service;
        protected readonly IApiMapper<TDto, TRequest, TResponse> Mapper;
        private readonly IOutputCacheStore _cacheStore;

        protected GenericController(
            IGenericService<TDto> service,
            IApiMapper<TDto, TRequest, TResponse> mapper,
            IOutputCacheStore cacheStore)
        {
            Service = service;
            Mapper = mapper;
            _cacheStore = cacheStore;
        }

        [HttpGet]
        [OutputCache(PolicyName = "CacheGet")]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = await Service.GetAllAsync();
            if (!result.IsSuccessful)
                return BadRequest(result.Error);

            var response = Mapper.ToResponseList(result.Data!);
            return Ok(response);
        }

        [HttpGet("paged")]
        [OutputCache(PolicyName = "CacheGet")]
        public virtual async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            var result = await Service.GetPagedAsync(request);
            if (!result.IsSuccessful)
                return BadRequest(result.Error);

            var pagedResponse = new PagedResult<TResponse>
            {
                Items = Mapper.ToResponseList(result.Data!.Items),
                Page = result.Data.Page,
                PageSize = result.Data.PageSize,
                TotalCount = result.Data.TotalCount
            };
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        [OutputCache(PolicyName = "CacheGet")]
        public virtual async Task<IActionResult> GetById(string id)
        {
            var result = await Service.GetByIdAsync(id);
            if (!result.IsSuccessful)
                return NotFound(result.Error);

            var response = Mapper.ToResponse(result.Data!);
            return Ok(response);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TRequest request)
        {
            var dto = Mapper.ToDto(request);
            var result = await Service.CreateAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result.Error);

            await InvalidateCacheAsync();
            var response = Mapper.ToResponse(result.Data!);
            return CreatedAtAction(nameof(GetById), new { id = GetId(response) }, response);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TRequest request)
        {
            var dto = Mapper.ToDto(request);
            var result = await Service.UpdateAsync(id, dto);
            if (!result.IsSuccessful)
                return NotFound(result.Error);

            await InvalidateCacheAsync();
            var response = Mapper.ToResponse(result.Data!);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            var result = await Service.DeleteAsync(id);
            if (!result.IsSuccessful)
                return NotFound(result.Error);

            await InvalidateCacheAsync();
            return NoContent();
        }

        protected async Task InvalidateCacheAsync()
        {
            await _cacheStore.EvictByTagAsync("all", default);
        }

        protected virtual string GetId(TResponse response)
        {
            var idProp = typeof(TResponse).GetProperty("Id");
            return idProp?.GetValue(response)?.ToString() ?? string.Empty;
        }
    }
}
