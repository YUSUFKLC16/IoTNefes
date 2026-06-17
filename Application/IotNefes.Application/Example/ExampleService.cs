using IotNefes.Abstractions.Example;
using IotNefes.Abstractions.Example.Dto;
using IotNefes.Common;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;
using ExampleEntity = IotNefes.Domain.Example.Example;

namespace IotNefes.Application.Example
{
    public class ExampleService : GenericService<ExampleDto, ExampleEntity>, IExampleService, IScopedService
    {
        public ExampleService(
            IGenericRepository<ExampleEntity> repository,
            IEntityMapper<ExampleDto, ExampleEntity> mapper,
            ILogger<ExampleService> logger)
            : base(repository, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ExampleDto>> CreateAsync(ExampleDto dto)
        {
            var exists = await Repository.AnyAsync(x => x.Name == dto.Name);
            if (exists)
                return ServiceResponse<ExampleDto>.Fail("Bu isimde bir kayıt zaten mevcut.", "DUPLICATE_NAME");

            dto.IsActive = true;
            return await base.CreateAsync(dto);
        }

        public override async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null)
                return ServiceResponse.Fail("Kayıt bulunamadı.", "NOT_FOUND");

            if (entity.IsActive)
            {
                Logger.LogWarning("Active example {Id} soft-deleted instead of hard delete", id);
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await Repository.UpdateAsync(entity);
                return ServiceResponse.Success();
            }

            return await base.DeleteAsync(id);
        }
    }
}