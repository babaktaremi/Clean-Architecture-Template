namespace Application.Contracts.Persistence
{
   public interface IAsyncRepository<T> where T:class
   {
       Task<List<T>> ListAllAsync();
       Task AddAsync(T entity);
       Task UpdateAsync(T entity);
       Task DeleteAsync(T entity);
   }
}
