using Infrastructure.Repositories.MongoDb.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.MongoDb.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            Context = context;
        }

        public virtual void Add(TEntity obj)
        {
            ConfigDbSet();
            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        private void ConfigDbSet()
        {
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);

            if (string.IsNullOrEmpty(DbSet.CollectionNamespace.CollectionName))
            {
                Context.CreateCollection(typeof(TEntity).Name);
            }
        }


        public async Task<IEnumerable<TEntity>> GetManyFiltered(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken)
        {
            ConfigDbSet();
            return (await DbSet.FindAsync(filter)).ToList(cancellationToken);
        }

        public async Task<TEntity> GetOneFiltered(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken)
        {
            ConfigDbSet();
            return (await DbSet.FindAsync(filter)).SingleOrDefault(cancellationToken);
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            ConfigDbSet();
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            ConfigDbSet();
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            ConfigDbSet();
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        public virtual void Remove(Guid id)
        {
            ConfigDbSet();
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        
    }
}
