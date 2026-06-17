using System.Linq.Expressions;
using IotNefes.Common;
using IotNefes.Infastructure.Abstraction;
using IotNefes.Domain;
using IotNefes.Persistance.Contexts;
using MongoDB.Driver;

namespace IotNefes.Persistance.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<T>();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var filter = predicate ?? (_ => true);
            var totalCount = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var items = await _collection.Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var list = entities.ToList();
            foreach (var entity in list)
                entity.CreatedAt = DateTime.UtcNow;

            await _collection.InsertManyAsync(list, cancellationToken: cancellationToken);
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate is null
                ? await _collection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken)
                : await _collection.CountDocumentsAsync(predicate, cancellationToken: cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).AnyAsync(cancellationToken);
        }
    }
}
