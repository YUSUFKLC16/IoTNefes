using IotNefes.Abstractions.Humidity.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class HumidityApiMapper : IApiMapper<HumidityDto, HumidityRequest, HumidityResponse>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(HumidityDto.Id))]
        [MapperIgnoreTarget(nameof(HumidityDto.CreatedAt))]
        [MapperIgnoreTarget(nameof(HumidityDto.UpdatedAt))]
        public partial HumidityDto ToDto(HumidityRequest request);
        public partial HumidityResponse ToResponse(HumidityDto dto);
        public partial List<HumidityResponse> ToResponseList(List<HumidityDto> dtos);
    }
}
