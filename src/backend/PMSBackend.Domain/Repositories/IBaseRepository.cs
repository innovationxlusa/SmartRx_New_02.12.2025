namespace PMSBackend.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetDetailsByIdAsync(long id);
    }
}
