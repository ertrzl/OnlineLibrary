using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Persistence.Implementations.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authors;
        public AuthorService(IAuthorRepository authors)
        {
            _authors = authors;
        }
        public void CreateAuthor(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
                throw new Exception("Validation Error: Author name cannot be empty!");
            author.Name = author.Name.Trim();

            if (!string.IsNullOrWhiteSpace(author.Surname))
                author.Surname = author.Surname.Trim();
            else
                author.Surname = null;

            var isDuplicate = _authors.GetAll()
            .Any(a => a.Name.ToLower() == author.Name.ToLower() &&
                      a.Surname == author.Surname);

            if (isDuplicate)
            {
                string fullName = author.Surname != null ? $"{author.Name} {author.Surname}" : author.Name;
                throw new Exception($"Conflict Error: Author '{fullName}' already exists in the system!");
            }

            _authors.Add(author);
            _authors.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            if (id <= 0)
                throw new Exception("Error: ID cannot be zero!");

            var author = _authors.GetById(id, isTracking: true);
            if (author == null)
                throw new Exception("Author not found!");

            var hasBooks = _authors.GetById(id).Books?.Any() ?? false;
            if (hasBooks)
                throw new Exception("Conflict Error: Cannot delete this author because they have associated books!");

            _authors.Delete(author);
            _authors.SaveChanges();
        }

        public List<Author> ShowAllAuthors()
        {
            var authors = _authors.GetAllWithBooks();

            return authors;
        }
        public Author GetByAuthorId(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Error: ID cannot be zero or negative!");
            }

            var author = _authors.GetById(id, isTracking: false);

            if (author == null)
            {
                throw new Exception($"Error: Author with ID {id} was not found!");
            }

            return author;
        }
    }
}

