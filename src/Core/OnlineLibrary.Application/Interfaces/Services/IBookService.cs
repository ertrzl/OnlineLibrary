using OnlineLibrary.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Application.Interfaces.Services
{
    public interface IBookService
    {
        void CreateBook(Book book);
        void DeleteBook(int id);
        Book? GetBookById(int id, bool withDate = false);
        List<Book> GetAll();
        List<Book> GetAllBooksWithAuthor();
        List<Book> GetBooksByAuthorId(int authorId);

    }
}
