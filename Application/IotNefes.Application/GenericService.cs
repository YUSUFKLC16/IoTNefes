using IotNefes.Abstractions;
using IotNefes.Common;
using IotNefes.Domain;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.Logging;

namespace IotNefes.Application
{
    public abstract class GenericService<TDto, TEntity> : IGenericService<TDto>
        where TDto : class
        where TEntity : BaseEntity
    {
        protected readonly IGenericRepository<TEntity> Repository;
        protected readonly IEntityMapper<TDto, TEntity> Mapper;
        protected readonly ILogger Logger;

        protected GenericService(
            IGenericRepository<TEntity> repository,
            IEntityMapper<TDto, TEntity> mapper,
            ILogger logger)
        {
            Repository = repository;
            Mapper = mapper;
            Logger = logger;
        }

        public virtual async Task<ServiceResponse<List<TDto>>> GetAllAsync()
        {
            try
            {
                var entities = await Repository.GetAllAsync();
                var dtos = Mapper.ToDtoList(entities);
                return ServiceResponse<List<TDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving all {Entity}", typeof(TEntity).Name);
                return ServiceResponse<List<TDto>>.Fail("Kayıtlar getirilirken bir hata oluştu.", "GET_ALL_ERROR");
            }
        }

        public virtual async Task<ServiceResponse<PagedResult<TDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var pagedEntities = await Repository.GetPagedAsync(request.Page, request.PageSize);
                var dtos = Mapper.ToDtoList(pagedEntities.Items);
                var result = new PagedResult<TDto>
                {
                    Items = dtos,
                    Page = pagedEntities.Page,
                    PageSize = pagedEntities.PageSize,
                    TotalCount = pagedEntities.TotalCount
                };
                return ServiceResponse<PagedResult<TDto>>.Success(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving paged {Entity}", typeof(TEntity).Name);
                return ServiceResponse<PagedResult<TDto>>.Fail("Kayıtlar getirilirken bir hata oluştu.", "GET_PAGED_ERROR");
            }
        }

        public virtual async Task<ServiceResponse<TDto>> GetByIdAsync(string id)
        {
            try
            {
                var entity = await Repository.GetByIdAsync(id);
                if (entity is null)
                    return ServiceResponse<TDto>.Fail("Kayıt bulunamadı.", "NOT_FOUND");

                var dto = Mapper.ToDto(entity);
                return ServiceResponse<TDto>.Success(dto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving {Entity} with id {Id}", typeof(TEntity).Name, id);
                return ServiceResponse<TDto>.Fail("Kayıt getirilirken bir hata oluştu.", "GET_ERROR");
            }
        }

        public virtual async Task<ServiceResponse<TDto>> CreateAsync(TDto dto)
        {
            try
            {
                var entity = Mapper.ToEntity(dto);
                var created = await Repository.AddAsync(entity);
                var result = Mapper.ToDto(created);
                return ServiceResponse<TDto>.Success(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating {Entity}", typeof(TEntity).Name);
                return ServiceResponse<TDto>.Fail("Kayıt oluşturulurken bir hata oluştu.", "CREATE_ERROR");
            }
        }

        public virtual async Task<ServiceResponse<TDto>> UpdateAsync(string id, TDto dto)
        {
            try
            {
                var entity = await Repository.GetByIdAsync(id);
                if (entity is null)
                    return ServiceResponse<TDto>.Fail("Kayıt bulunamadı.", "NOT_FOUND");

                Mapper.UpdateEntity(dto, entity);
                entity.UpdatedAt = DateTime.UtcNow;

                var updated = await Repository.UpdateAsync(entity);
                var result = Mapper.ToDto(updated);
                return ServiceResponse<TDto>.Success(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating {Entity} with id {Id}", typeof(TEntity).Name, id);
                return ServiceResponse<TDto>.Fail("Kayıt güncellenirken bir hata oluştu.", "UPDATE_ERROR");
            }
        }

        public virtual async Task<ServiceResponse> DeleteAsync(string id)
        {
            try
            {
                var deleted = await Repository.DeleteAsync(id);
                if (!deleted)
                    return ServiceResponse.Fail("Kayıt bulunamadı.", "NOT_FOUND");

                return ServiceResponse.Success();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting {Entity} with id {Id}", typeof(TEntity).Name, id);
                return ServiceResponse.Fail("Kayıt silinirken bir hata oluştu.", "DELETE_ERROR");
            }
        }
    }
}