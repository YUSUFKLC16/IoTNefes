using IotNefes.Abstractions.Example.Dto;
using IotNefes.Infastructure.Abstraction;
using IotNefes.WebApi.src.Models;
using Riok.Mapperly.Abstractions;

namespace IotNefes.WebApi.src.Mapping
{
    [Mapper]
    public partial class ExampleApiMapper : IApiMapper<ExampleDto, ExampleRequest, ExampleResponse>, ISingletonService
    {
        [MapperIgnoreTarget(nameof(ExampleDto.Id))]
        [MapperIgnoreTarget(nameof(ExampleDto.CreatedAt))]
        [MapperIgnoreTarget(nameof(ExampleDto.UpdatedAt))]
        public partial ExampleDto ToDto(ExampleRequest request);
        public partial ExampleResponse ToResponse(ExampleDto dto);
        public partial List<ExampleResponse> ToResponseList(List<ExampleDto> dtos);
    }
}
