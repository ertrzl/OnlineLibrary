using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Persistence.DAL;


namespace OnlineLibrary.Persistence.Implementations.Repositories
{
    public class BookRepository: GenericRepository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public List<Book> GetBooksWithAuthors()
        {
            return _table
            .Include(b => b.Author)
            .ToList();
        }
    }
}
