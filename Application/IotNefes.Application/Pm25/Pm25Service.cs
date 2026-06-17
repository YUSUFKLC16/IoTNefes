using IotNefes.Abstractions.Pm25;
using IotNefes.Abstractions.Pm25.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using Pm25Entity = IotNefes.Domain.Pm25.Pm25;

namespace IotNefes.Application.Pm25
{
    public class Pm25Service : GenericService<Pm25Dto, Pm25Entity>, IPm25Service, IScopedService
    {
        public Pm25Service(
            IGenericRepository<Pm25Entity> repository,
            IEntityMapper<Pm25Dto, Pm25Entity> mapper,
            ILogger<Pm25Service> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
