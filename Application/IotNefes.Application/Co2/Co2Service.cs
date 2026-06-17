using IotNefes.Abstractions.Co2;
using IotNefes.Abstractions.Co2.Dto;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using Co2Entity = IotNefes.Domain.Co2.Co2;

namespace IotNefes.Application.Co2
{
    public class Co2Service : GenericService<Co2Dto, Co2Entity>, ICo2Service, IScopedService
    {
        public Co2Service(
            IGenericRepository<Co2Entity> repository,
            IEntityMapper<Co2Dto, Co2Entity> mapper,
            ILogger<Co2Service> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
