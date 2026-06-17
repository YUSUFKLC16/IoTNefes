using IotNefes.Abstractions.AirTemperature.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class AirTemperatureApiMapper : IApiMapper<AirTemperatureDto, AirTemperatureRequest, AirTemperatureResponse>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(AirTemperatureDto.Id))]
        [MapperIgnoreTarget(nameof(AirTemperatureDto.CreatedAt))]
        [MapperIgnoreTarget(nameof(AirTemperatureDto.UpdatedAt))]
        public partial AirTemperatureDto ToDto(AirTemperatureRequest request);
        public partial AirTemperatureResponse ToResponse(AirTemperatureDto dto);
        public partial List<AirTemperatureResponse> ToResponseList(List<AirTemperatureDto> dtos);
    }
}
