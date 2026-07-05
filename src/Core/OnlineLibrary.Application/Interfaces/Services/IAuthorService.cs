using OnlineLibrary.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        void CreateAuthor(Author author);
        void DeleteAuthor(int id);

        List<Author> ShowAllAuthors();

        Author? GetByAuthorId(int id);





    }
}
