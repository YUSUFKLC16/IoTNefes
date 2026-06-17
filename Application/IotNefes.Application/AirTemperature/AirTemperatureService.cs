using IotNefes.Abstractions.AirTemperature;
using IotNefes.Abstractions.AirTemperature.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using AirTemperatureEntity = IotNefes.Domain.AirTemperature.AirTemperature;

namespace IotNefes.Application.AirTemperature
{
    public class AirTemperatureService : GenericService<AirTemperatureDto, AirTemperatureEntity>, IAirTemperatureService, IScopedService
    {
        public AirTemperatureService(
            IGenericRepository<AirTemperatureEntity> repository,
            IEntityMapper<AirTemperatureDto, AirTemperatureEntity> mapper,
            ILogger<AirTemperatureService> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
