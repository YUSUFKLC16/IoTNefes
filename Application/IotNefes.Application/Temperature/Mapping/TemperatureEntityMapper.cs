using IotNefes.Abstractions.Temperature.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using TemperatureEntity = IotNefes.Domain.Temperature.Temperature;

namespace IotNefes.Application.Temperature.Mapping
{
    [Mapper]
    public partial class TemperatureEntityMapper : IEntityMapper<TemperatureDto, TemperatureEntity>, ISingletonService
    {
        public partial TemperatureDto ToDto(TemperatureEntity entity);

        [MapperIgnoreTarget(nameof(TemperatureEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(TemperatureEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(TemperatureDto.CreatedAt))]
        [MapperIgnoreSource(nameof(TemperatureDto.UpdatedAt))]
        public partial TemperatureEntity ToEntity(TemperatureDto dto);

        public partial List<TemperatureDto> ToDtoList(List<TemperatureEntity> entities);

        [MapperIgnoreTarget(nameof(TemperatureEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(TemperatureEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(TemperatureDto.CreatedAt))]
        [MapperIgnoreSource(nameof(TemperatureDto.UpdatedAt))]
        public partial TemperatureEntity UpdateEntity(TemperatureDto dto, TemperatureEntity entity);
    }
}
