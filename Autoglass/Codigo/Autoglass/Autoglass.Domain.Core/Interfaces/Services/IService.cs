using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Domain.Core.Interfaces.Services
{
    public interface IService<TEntity> : IDisposable where TEntity : class
    {
        Task Inactivate(TEntity obj);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Insert(TEntity obj);
        Task<TEntity> Update(TEntity obj);
        void showError(string propertyType, string errorMenssage);
        //Task<IEnumerable<TEntity>> UpdateRange(IEnumerable<TEntity> obj);
        //Task<TEntity?> GetByIdActive(Guid id);
        Task<TEntity?> GetByIdActive(long id);
        //Task<IEnumerable<TEntity>> InsertRange(IEnumerable<TEntity> obj);
        //Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate);
        //Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate);
        //Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        //Task DeleteRange(IEnumerable<TEntity> obj);
        //Task Delete(Guid id);
    }
}
