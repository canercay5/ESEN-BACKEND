using ESEN.Domain.Entities;
using ESEN.Domain.Interfaces;
using ESEN.Infrastructure.Data;
using MongoDB.Driver;

namespace ESEN.Infrastructure.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(MongoDbContext context)
        {
            string collectionName = typeof(T).Name + "s";

            _collection = context.Database.GetCollection<T>(collectionName);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.Find(e => e.Id == id && !e.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(e => !e.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var update = Builders<T>.Update.Set(e => e.IsDeleted, true);
            await _collection.UpdateOneAsync(e => e.Id == id, update);
        }
    }
}