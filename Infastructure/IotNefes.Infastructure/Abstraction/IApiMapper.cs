namespace IotNefes.Infastructure.Abstraction
{
    public interface IApiMapper<TDto, TRequest, TResponse>
    {
        TDto ToDto(TRequest request);
        TResponse ToResponse(TDto dto);
        List<TResponse> ToResponseList(List<TDto> dtos);
    }
}
