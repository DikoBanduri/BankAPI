using Bank.Service.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bank.Repository;

internal abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly BankDbContext _context;
    protected readonly DbSet<T> _dbSet;

    internal RepositoryBase(BankDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public T Get(params object[] id) =>
       _dbSet.Find(id) ?? throw new KeyNotFoundException($"Record with key {id} not found");

    public IQueryable<T> Set(Expression<Func<T, bool>> predicate) =>
        _dbSet.Where(predicate);

    public IQueryable<T> Set() =>
        _dbSet;

    public void Insert(T entity) =>
        _dbSet.Add(entity);

    //public void Update(T entity)
    //{
    //    _dbSet.Attach(entity);
    //    _context.Entry(entity).State = EntityState.Modified;
    //}

    public void Update(T entity)
    {
        var keyProperty = _context.Model.FindEntityType(typeof(T))
                                        ?.FindPrimaryKey()
                                        ?.Properties
                                        ?.FirstOrDefault();

        if (keyProperty == null)
        {
            throw new InvalidOperationException("Could not find the primary key.");
        }

        var keyValue = keyProperty?.PropertyInfo?.GetValue(entity);

        var existingEntity = _dbSet.Find(keyValue);
        if (existingEntity == null)
        {
            throw new InvalidOperationException("Entity not found in the database.");
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
    }


    public void InsertOrUpdate(T entity)
    {
        var entry = _context.Entry(entity);
        if (entry == null || entry.State == EntityState.Detached)
        {
            Insert(entity);
        }
        else
        {
            Update(entity);
        }
    }

    public void Delete(object id) =>
        Delete(Get(id));

    public void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    public async Task<T> GetAsync(params object[] id) =>
        await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Record with key {id} not found");

    //public async Task<List<T>> SetAsync(Expression<Func<T, bool>> predicate) =>
    //  await _dbSet.Where(predicate).ToListAsync();

    public async Task InsertAsync(T entity) =>
        await _dbSet.AddAsync(entity);

  

    public async Task InsertOrUpdateAsync(T entity)
    {
        var entry = _context.Entry(entity);
        if (entry == null || entry.State == EntityState.Detached)
        {
            await InsertAsync(entity);
        }
        else
        {
             Update(entity);
        }
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await GetAsync(id);
        await DeleteAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }
}
