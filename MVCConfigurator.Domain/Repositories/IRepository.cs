using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        void Delete(T entity);
        void Update(T entity);
    }
}
