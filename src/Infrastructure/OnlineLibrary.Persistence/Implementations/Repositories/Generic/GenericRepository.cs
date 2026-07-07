using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Persistence.DAL;


namespace OnlineLibrary.Persistence.Implementations.Repositories;
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _table;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _table = context.Set<T>();    
        }


        public void Add(T entity)
        {
            _table.Add(entity);
        }

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public virtual List<T> GetAll()
        {
            return _table.AsNoTracking().ToList();
        }

        public T? GetById(int id, bool isTracking = false)
        {

            if (isTracking)
            {
                return _table.FirstOrDefault(x => x.Id == id);
            }

            return _table.AsNoTracking().FirstOrDefault(x => x.Id == id);
                
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _table.Update(entity);
        }
    }

