using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.MongoDb.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);

        Task<IEnumerable<TEntity>> GetManyFiltered(Expression<Func<TEntity,bool>> filter, CancellationToken cancellationToken);

        Task<TEntity> GetOneFiltered(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Remove(Guid id);
    }
}
