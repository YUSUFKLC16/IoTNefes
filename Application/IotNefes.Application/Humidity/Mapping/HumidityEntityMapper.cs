using IotNefes.Abstractions.Humidity.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using HumidityEntity = IotNefes.Domain.Humidity.Humidity;

namespace IotNefes.Application.Humidity.Mapping
{
    [Mapper]
    public partial class HumidityEntityMapper : IEntityMapper<HumidityDto, HumidityEntity>, ISingletonService
    {
        public partial HumidityDto ToDto(HumidityEntity entity);

        [MapperIgnoreTarget(nameof(HumidityEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(HumidityEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(HumidityDto.CreatedAt))]
        [MapperIgnoreSource(nameof(HumidityDto.UpdatedAt))]
        public partial HumidityEntity ToEntity(HumidityDto dto);

        public partial List<HumidityDto> ToDtoList(List<HumidityEntity> entities);

        [MapperIgnoreTarget(nameof(HumidityEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(HumidityEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(HumidityDto.CreatedAt))]
        [MapperIgnoreSource(nameof(HumidityDto.UpdatedAt))]
        public partial HumidityEntity UpdateEntity(HumidityDto dto, HumidityEntity entity);
    }
}
