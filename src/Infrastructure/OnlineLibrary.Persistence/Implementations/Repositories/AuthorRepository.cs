using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Persistence.DAL;

namespace OnlineLibrary.Persistence.Implementations.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {

        public AuthorRepository(AppDbContext context) : base(context)
        {

        }
        public List<Author> GetAllWithBooks()
        {
            return _table.Include(a => a.Books).ToList();
        }

        public Author? GetByIdWithBooks(int id, bool isTracking = false)
        {
            var query = _table.Include(a => a.Books).Where(a => a.Id == id);
            if (!isTracking)
                query = query.AsNoTracking();
            return query.FirstOrDefault();
        }


    }
}