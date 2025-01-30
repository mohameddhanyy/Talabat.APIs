using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetWithSpecsAsync(ISpecifications<T> specs);
        Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> specs);

        Task<int> GetPaginationCount(ISpecifications<T> specs);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);



    }
}
