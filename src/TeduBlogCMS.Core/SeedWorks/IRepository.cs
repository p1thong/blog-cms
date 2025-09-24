using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TeduBlogCMS.Core.SeedWorks
{
    public interface IRepository<T, key>
        where T : class
    {
        Task<T> GetByIdAsync(Key id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
