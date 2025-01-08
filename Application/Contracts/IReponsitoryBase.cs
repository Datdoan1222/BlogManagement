using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogManagement.Contracts
{
    public interface  IReponsitoryBase<T> where T : class 
    {
        Task<ICollection<T>> FindAllParent();
        Task<T> FindById(long id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
        Task<bool> IsExists(long id);

    }

}
