using IotNefes.Abstractions.Pm10.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using Pm10Entity = IotNefes.Domain.Pm10.Pm10;

namespace IotNefes.Application.Pm10.Mapping
{
    [Mapper]
    public partial class Pm10EntityMapper : IEntityMapper<Pm10Dto, Pm10Entity>, ISingletonService
    {
        public partial Pm10Dto ToDto(Pm10Entity entity);

        [MapperIgnoreTarget(nameof(Pm10Entity.CreatedAt))]
        [MapperIgnoreTarget(nameof(Pm10Entity.UpdatedAt))]
        [MapperIgnoreSource(nameof(Pm10Dto.CreatedAt))]
        [MapperIgnoreSource(nameof(Pm10Dto.UpdatedAt))]
        public partial Pm10Entity ToEntity(Pm10Dto dto);

        public partial List<Pm10Dto> ToDtoList(List<Pm10Entity> entities);

        [MapperIgnoreTarget(nameof(Pm10Entity.CreatedAt))]
        [MapperIgnoreTarget(nameof(Pm10Entity.UpdatedAt))]
        [MapperIgnoreSource(nameof(Pm10Dto.CreatedAt))]
        [MapperIgnoreSource(nameof(Pm10Dto.UpdatedAt))]
        public partial Pm10Entity UpdateEntity(Pm10Dto dto, Pm10Entity entity);
    }
}
