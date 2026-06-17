namespace IotNefes.Infastructure.Abstraction
{
    public interface IEntityMapper<TDto, TEntity>
    {
        TDto ToDto(TEntity entity);
        TEntity ToEntity(TDto dto);
        List<TDto> ToDtoList(List<TEntity> entities);
        TEntity UpdateEntity(TDto dto, TEntity entity);
    }
}
