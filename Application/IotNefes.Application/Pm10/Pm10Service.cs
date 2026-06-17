using IotNefes.Abstractions.Pm10;
using IotNefes.Abstractions.Pm10.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using Pm10Entity = IotNefes.Domain.Pm10.Pm10;

namespace IotNefes.Application.Pm10
{
    public class Pm10Service : GenericService<Pm10Dto, Pm10Entity>, IPm10Service, IScopedService
    {
        public Pm10Service(
            IGenericRepository<Pm10Entity> repository,
            IEntityMapper<Pm10Dto, Pm10Entity> mapper,
            ILogger<Pm10Service> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
