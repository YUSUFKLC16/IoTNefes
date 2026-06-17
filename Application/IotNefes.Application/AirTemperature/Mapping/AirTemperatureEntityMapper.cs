using IotNefes.Abstractions.AirTemperature.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using AirTemperatureEntity = IotNefes.Domain.AirTemperature.AirTemperature;

namespace IotNefes.Application.AirTemperature.Mapping
{
    [Mapper]
    public partial class AirTemperatureEntityMapper : IEntityMapper<AirTemperatureDto, AirTemperatureEntity>, ISingletonService
    {
        public partial AirTemperatureDto ToDto(AirTemperatureEntity entity);

        [MapperIgnoreTarget(nameof(AirTemperatureEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(AirTemperatureEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(AirTemperatureDto.CreatedAt))]
        [MapperIgnoreSource(nameof(AirTemperatureDto.UpdatedAt))]
        public partial AirTemperatureEntity ToEntity(AirTemperatureDto dto);

        public partial List<AirTemperatureDto> ToDtoList(List<AirTemperatureEntity> entities);

        [MapperIgnoreTarget(nameof(AirTemperatureEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(AirTemperatureEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(AirTemperatureDto.CreatedAt))]
        [MapperIgnoreSource(nameof(AirTemperatureDto.UpdatedAt))]
        public partial AirTemperatureEntity UpdateEntity(AirTemperatureDto dto, AirTemperatureEntity entity);
    }
}
