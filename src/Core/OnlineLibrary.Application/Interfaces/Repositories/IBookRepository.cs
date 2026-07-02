using OnlineLibrary.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Application.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        List<Book> GetBooksWithAuthors();
    }
}
