using IotNefes.Abstractions.Pm25.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class Pm25ApiMapper : IApiMapper<Pm25Dto, Pm25Request, Pm25Response>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(Pm25Dto.Id))]
        [MapperIgnoreTarget(nameof(Pm25Dto.CreatedAt))]
        [MapperIgnoreTarget(nameof(Pm25Dto.UpdatedAt))]
        public partial Pm25Dto ToDto(Pm25Request request);
        public partial Pm25Response ToResponse(Pm25Dto dto);
        public partial List<Pm25Response> ToResponseList(List<Pm25Dto> dtos);
    }
}
