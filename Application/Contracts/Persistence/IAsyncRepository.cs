using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
