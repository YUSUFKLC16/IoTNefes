using IotNefes.Abstractions.Co2.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class Co2ApiMapper : IApiMapper<Co2Dto, Co2Request, Co2Response>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(Co2Dto.Id))]
        [MapperIgnoreTarget(nameof(Co2Dto.CreatedAt))]
        [MapperIgnoreTarget(nameof(Co2Dto.UpdatedAt))]
        public partial Co2Dto ToDto(Co2Request request);
        public partial Co2Response ToResponse(Co2Dto dto);
        public partial List<Co2Response> ToResponseList(List<Co2Dto> dtos);
    }
}
