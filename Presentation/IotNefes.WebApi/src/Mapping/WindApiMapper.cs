using IotNefes.Abstractions.Wind.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class WindApiMapper : IApiMapper<WindDto, WindRequest, WindResponse>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(WindDto.Id))]
        [MapperIgnoreTarget(nameof(WindDto.CreatedAt))]
        [MapperIgnoreTarget(nameof(WindDto.UpdatedAt))]
        public partial WindDto ToDto(WindRequest request);
        public partial WindResponse ToResponse(WindDto dto);
        public partial List<WindResponse> ToResponseList(List<WindDto> dtos);
    }
}
