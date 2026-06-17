using IotNefes.Abstractions.Co2.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using Co2Entity = IotNefes.Domain.Co2.Co2;

namespace IotNefes.Application.Co2.Mapping
{
    [Mapper]
    public partial class Co2EntityMapper : IEntityMapper<Co2Dto, Co2Entity>, ISingletonService
    {
        public partial Co2Dto ToDto(Co2Entity entity);

        [MapperIgnoreTarget(nameof(Co2Entity.CreatedAt))]
        [MapperIgnoreTarget(nameof(Co2Entity.UpdatedAt))]
        [MapperIgnoreSource(nameof(Co2Dto.CreatedAt))]
        [MapperIgnoreSource(nameof(Co2Dto.UpdatedAt))]
        public partial Co2Entity ToEntity(Co2Dto dto);

        public partial List<Co2Dto> ToDtoList(List<Co2Entity> entities);

        [MapperIgnoreTarget(nameof(Co2Entity.CreatedAt))]
        [MapperIgnoreTarget(nameof(Co2Entity.UpdatedAt))]
        [MapperIgnoreSource(nameof(Co2Dto.CreatedAt))]
        [MapperIgnoreSource(nameof(Co2Dto.UpdatedAt))]
        public partial Co2Entity UpdateEntity(Co2Dto dto, Co2Entity entity);
    }
}
