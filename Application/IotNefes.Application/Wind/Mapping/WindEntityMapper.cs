using IotNefes.Abstractions.Wind.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using WindEntity = IotNefes.Domain.Wind.Wind;

namespace IotNefes.Application.Wind.Mapping
{
    [Mapper]
    public partial class WindEntityMapper : IEntityMapper<WindDto, WindEntity>, ISingletonService
    {
        public partial WindDto ToDto(WindEntity entity);

        [MapperIgnoreTarget(nameof(WindEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(WindEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(WindDto.CreatedAt))]
        [MapperIgnoreSource(nameof(WindDto.UpdatedAt))]
        public partial WindEntity ToEntity(WindDto dto);

        public partial List<WindDto> ToDtoList(List<WindEntity> entities);

        [MapperIgnoreTarget(nameof(WindEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(WindEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(WindDto.CreatedAt))]
        [MapperIgnoreSource(nameof(WindDto.UpdatedAt))]
        public partial WindEntity UpdateEntity(WindDto dto, WindEntity entity);
    }
}
