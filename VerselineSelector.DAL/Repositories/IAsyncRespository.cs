using System.Linq.Expressions;

namespace VerselineSelector.DAL.Repositories;

public interface IAsyncRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetEntity(int id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
}
