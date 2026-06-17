using IotNefes.Abstractions.Temperature;
using IotNefes.Abstractions.Temperature.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using TemperatureEntity = IotNefes.Domain.Temperature.Temperature;

namespace IotNefes.Application.Temperature
{
    public class TemperatureService : GenericService<TemperatureDto, TemperatureEntity>, ITemperatureService, IScopedService
    {
        public TemperatureService(
            IGenericRepository<TemperatureEntity> repository,
            IEntityMapper<TemperatureDto, TemperatureEntity> mapper,
            ILogger<TemperatureService> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
