using System.Linq.Expressions;

namespace VerselineSelector.DAL.Repositories;

public class EfRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _dBContext;

    public EfRepository(DbContext dBContext)
    {
        _dBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
    }

    public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate) => await _dBContext.Set<TEntity>().Where(predicate).ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAll() => await _dBContext.Set<TEntity>().ToListAsync();

    public async Task<TEntity?> GetEntity(int id) => await _dBContext.Set<TEntity>().FindAsync(id);
}