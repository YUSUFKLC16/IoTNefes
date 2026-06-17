using IotNefes.Abstractions.Example.Dto;

namespace IotNefes.Abstractions.Example
{
    public interface IExampleService : IGenericService<ExampleDto>
    {
        // Ekstra method'lar gerekirse buraya eklenecek
    }
}