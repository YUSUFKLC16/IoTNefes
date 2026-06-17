using IotNefes.Common;

namespace IotNefes.Abstractions
{
    public interface IGenericService<TDto> where TDto : class
    {
        Task<ServiceResponse<List<TDto>>> GetAllAsync();
        Task<ServiceResponse<PagedResult<TDto>>> GetPagedAsync(PagedRequest request);
        Task<ServiceResponse<TDto>> GetByIdAsync(string id);
        Task<ServiceResponse<TDto>> CreateAsync(TDto dto);
        Task<ServiceResponse<TDto>> UpdateAsync(string id, TDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
    }
}