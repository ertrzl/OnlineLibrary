using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Domain.Enums;

namespace OnlineLibrary.Persistence.Implementations.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _books;
        private readonly IReservedItemRepository _reservations;
        private readonly IAuthorRepository _authors;

        public BookService(IBookRepository books, IReservedItemRepository reservations, IAuthorRepository authors)
        {
            _books = books;
            _reservations = reservations;
            _authors = authors;
        }
        public void CreateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Name))
                throw new Exception(" Book name cannot be empty!");

            if (book.PageCount <= 0)
                throw new Exception("Page count must be greater than zero");

            var author = _authors.GetById(book.AuthorId, isTracking: false);
            if (author == null)
                throw new Exception($"Not Found Error: Author with ID {book.AuthorId} does not exist in the system!");

            var isDuplicate = _books.GetAll()
            .Any(b => b.Name.Trim().ToLower() == book.Name.Trim().ToLower() && b.AuthorId == book.AuthorId);

            if (isDuplicate)
                throw new Exception($"Conflict Error: A book with the name '{book.Name}' already exists for this author!");

            book.Name = book.Name.Trim();

            _books.Add(book);
            _books.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            if (id <= 0)
                throw new Exception("Error: ID cannot be zero!");

            var book = _books.GetById(id, isTracking: true);
            if (book is null)
                throw new Exception($"Error: Book with ID {id} was not found!");
            var hasActiveReservation = _reservations.GetAll()
                 .Any(r => r.BookId == id
                       && (r.Status == Status.Confirmed || r.Status == Status.Started));
            if (hasActiveReservation)
                throw new Exception($"Error: Book with ID {id} cannot be deleted because it has active reservations!");

            _books.Delete(book);
            _books.SaveChanges();
        }

        public List<Book> GetAll()
        {
            return _books.GetAll();
        }

        public List<Book> GetAllBooksWithAuthor()
        {
            return _books.GetBooksWithAuthors();
        }

        public Book GetBookById(int id)
        {
            if (id <= 0)
                throw new Exception("Error: ID cannot be zero!");
            var book = _books.GetById(id, isTracking: false);

            if (book is null)
                throw new Exception($"Error: Book with ID {id} was not found!");

            return book;
        }

       
    }
}
