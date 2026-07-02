using OnlineLibrary.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        void Add(T entity);
        List<T> GetAll();
        T? GetById(int id, bool isTracking = false);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
