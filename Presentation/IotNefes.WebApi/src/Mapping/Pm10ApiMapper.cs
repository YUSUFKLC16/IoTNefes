using IotNefes.Abstractions.Pm10.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class Pm10ApiMapper : IApiMapper<Pm10Dto, Pm10Request, Pm10Response>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(Pm10Dto.Id))]
        [MapperIgnoreTarget(nameof(Pm10Dto.CreatedAt))]
        [MapperIgnoreTarget(nameof(Pm10Dto.UpdatedAt))]
        public partial Pm10Dto ToDto(Pm10Request request);
        public partial Pm10Response ToResponse(Pm10Dto dto);
        public partial List<Pm10Response> ToResponseList(List<Pm10Dto> dtos);
    }
}
