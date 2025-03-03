using System.Linq.Expressions;

namespace Bank.Service.Interfaces.Repository;

public interface IRepositoryBase<T> where T : class
{
    T Get(params object[] id);
    IQueryable<T> Set(Expression<Func<T, bool>> predicate);
    IQueryable<T> Set();
    void Insert(T entity);
    void Update(T entity);
    void InsertOrUpdate(T entity);
    void Delete(object id);
    void Delete(T entity);

    Task<T> GetAsync(params object[] id);
    //Task<List<T>> SetAsync(Expression<Func<T, bool>> predicate);
    Task InsertAsync(T entity);
    Task InsertOrUpdateAsync(T entity);
    Task DeleteAsync(object id);
    Task DeleteAsync(T entity);
}
