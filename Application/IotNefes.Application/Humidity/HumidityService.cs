using IotNefes.Abstractions.Humidity;
using IotNefes.Abstractions.Humidity.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using HumidityEntity = IotNefes.Domain.Humidity.Humidity;

namespace IotNefes.Application.Humidity
{
    public class HumidityService : GenericService<HumidityDto, HumidityEntity>, IHumidityService, IScopedService
    {
        public HumidityService(
            IGenericRepository<HumidityEntity> repository,
            IEntityMapper<HumidityDto, HumidityEntity> mapper,
            ILogger<HumidityService> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
