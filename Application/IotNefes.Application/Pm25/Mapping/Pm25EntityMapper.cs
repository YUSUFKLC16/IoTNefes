using IotNefes.Abstractions.Pm25.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using Pm25Entity = IotNefes.Domain.Pm25.Pm25;

namespace IotNefes.Application.Pm25.Mapping
{
    [Mapper]
    public partial class Pm25EntityMapper : IEntityMapper<Pm25Dto, Pm25Entity>, ISingletonService
    {
        public partial Pm25Dto ToDto(Pm25Entity entity);

        [MapperIgnoreTarget(nameof(Pm25Entity.CreatedAt))]
        [MapperIgnoreTarget(nameof(Pm25Entity.UpdatedAt))]
        [MapperIgnoreSource(nameof(Pm25Dto.CreatedAt))]
        [MapperIgnoreSource(nameof(Pm25Dto.UpdatedAt))]
        public partial Pm25Entity ToEntity(Pm25Dto dto);

        public partial List<Pm25Dto> ToDtoList(List<Pm25Entity> entities);

        [MapperIgnoreTarget(nameof(Pm25Entity.CreatedAt))]
        [MapperIgnoreTarget(nameof(Pm25Entity.UpdatedAt))]
        [MapperIgnoreSource(nameof(Pm25Dto.CreatedAt))]
        [MapperIgnoreSource(nameof(Pm25Dto.UpdatedAt))]
        public partial Pm25Entity UpdateEntity(Pm25Dto dto, Pm25Entity entity);
    }
}
