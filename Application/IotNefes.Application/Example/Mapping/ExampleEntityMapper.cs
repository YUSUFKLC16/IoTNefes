using IotNefes.Abstractions.Example.Dto;
using IotNefes.Infastructure.Abstraction;
using Riok.Mapperly.Abstractions;
using ExampleEntity = IotNefes.Domain.Example.Example;

namespace IotNefes.Application.Example.Mapping
{
    [Mapper]
    public partial class ExampleEntityMapper : IEntityMapper<ExampleDto, ExampleEntity>, ISingletonService
    {
        public partial ExampleDto ToDto(ExampleEntity entity);

        [MapperIgnoreTarget(nameof(ExampleEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(ExampleEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(ExampleDto.CreatedAt))]
        [MapperIgnoreSource(nameof(ExampleDto.UpdatedAt))]
        public partial ExampleEntity ToEntity(ExampleDto dto);

        public partial List<ExampleDto> ToDtoList(List<ExampleEntity> entities);

        [MapperIgnoreTarget(nameof(ExampleEntity.CreatedAt))]
        [MapperIgnoreTarget(nameof(ExampleEntity.UpdatedAt))]
        [MapperIgnoreSource(nameof(ExampleDto.CreatedAt))]
        [MapperIgnoreSource(nameof(ExampleDto.UpdatedAt))]
        public partial ExampleEntity UpdateEntity(ExampleDto dto, ExampleEntity entity);
    }
}
