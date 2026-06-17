using IotNefes.Abstractions.Temperature.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class TemperatureApiMapper : IApiMapper<TemperatureDto, TemperatureRequest, TemperatureResponse>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(TemperatureDto.Id))]
        [MapperIgnoreTarget(nameof(TemperatureDto.CreatedAt))]
        [MapperIgnoreTarget(nameof(TemperatureDto.UpdatedAt))]
        public partial TemperatureDto ToDto(TemperatureRequest request);
        public partial TemperatureResponse ToResponse(TemperatureDto dto);
        public partial List<TemperatureResponse> ToResponseList(List<TemperatureDto> dtos);
    }
}
