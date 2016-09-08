using System.Collections.Generic;

namespace AngularBlog.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        TEntity Add(TEntity item);
        bool Update(TEntity item);
        bool DeleteById(int id);
        bool Delete(TEntity item);
    }
}