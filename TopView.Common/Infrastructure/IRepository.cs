namespace TopView.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task Reset();
    Task<List<T>?> GetAllAsync();
    Task AddAsync(T entity);
    Task SaveAsync(T entity);
    Task RemoveAsync(T entity);
}
