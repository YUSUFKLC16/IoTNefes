using IotNefes.Abstractions.Wind;
using IotNefes.Abstractions.Wind.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using WindEntity = IotNefes.Domain.Wind.Wind;

namespace IotNefes.Application.Wind
{
    public class WindService : GenericService<WindDto, WindEntity>, IWindService, IScopedService
    {
        public WindService(
            IGenericRepository<WindEntity> repository,
            IEntityMapper<WindDto, WindEntity> mapper,
            ILogger<WindService> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
