using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Domain.Enums;
using OnlineLibrary.Persistence.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.ConsoleApp
{
    public class ManageMethod
    {
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly IReservedItemService _reservedService;

        public ManageMethod(IAuthorService authorService, IBookService bookService, IReservedItemService reservedService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _reservedService = reservedService;
        }
        public void CreateBook()
        {
            while (true)
            {
                Console.Write("Enter Book Name (or 'menu' to exit): ");
                string name = Console.ReadLine()?.Trim();
                if (name?.ToLower() == "menu") return;

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Error: Book name cannot be empty!\n");
                    continue;
                }

                Console.Write("Enter Page Count: ");
                if (!int.TryParse(Console.ReadLine(), out int pageCount) || pageCount <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid page count!\n");
                    continue;
                }

                Console.Write("Enter Author ID: ");
                if (!int.TryParse(Console.ReadLine(), out int authorId))
                {
                    Console.WriteLine("Error: Invalid Author ID format!\n");
                    continue;
                }

                try
                {

                    var author = _authorService.GetByAuthorId(authorId);
                    if (author == null)
                    {
                        Console.WriteLine($"Error: Author with ID {authorId} does not exist! Try again.\n");
                        continue;
                    }

                    var newBook = new Book { Name = name, PageCount = pageCount, AuthorId = authorId };
                    _bookService.CreateBook(newBook);
                    Console.WriteLine("Book created successfully!");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n");
                }
            }
        }
        public void DeleteBook()
        {
            while (true)
            {
                Console.Write("Enter the ID of the book you want to delete (or 'menu' to exit): ");
                string input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }
                if (!int.TryParse(input, out int id))
                {
                    Console.WriteLine("Error: Please enter a valid book ID! Try again.\n");
                    continue;
                }
                try
                {
                    _bookService.DeleteBook(id);

                    Console.WriteLine($"\nSuccess: Book with ID {id} deleted successfully!");
                    break;
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"\nService {ex.Message} Try again.\n");
                }
            }
        }
        public void GetBookById()
        {
            while (true)
            {
                Console.Write("Enter Book ID (or 'menu' to exit): ");
                string input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }
                if (!int.TryParse(input, out int id) || id <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid book ID! Try again.\n");
                    continue;
                }
                try
                {
                    Book book = _bookService.GetBookById(id);

                    Console.WriteLine("\n=== BOOK INFORMATION ===");
                    Console.WriteLine($"ID: {book.Id}");
                    Console.WriteLine($"Name: {book.Name}");
                    Console.WriteLine($"Page Count: {book.PageCount}");
                    Console.WriteLine($"Author ID: {book.AuthorId}");

                    var history = _reservedService.GetAllReservationsOrderedByStatus()
                        .Where(r => r.BookId == id)
                        .ToList();


                    Console.WriteLine("\n=== RESERVATION HISTORY ===");
                    if (history.Count == 0)
                    {
                        Console.WriteLine("No reservation history found for this book.");
                    }
                    else
                    {
                        foreach (var res in history)
                        {
                            Console.WriteLine($"- User FIN: {res.FinCode} | Status: {res.Status} | {res.StartDate.ToShortDateString()} -> {res.EndDate.ToShortDateString()}");
                        }
                    }
                    break;
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"\nService Error: {ex.Message} Try again.\n");
                }
            }
        }
        public void ShowAllBooks()
        {
            Console.WriteLine("=== ALL BOOKS LIST ===");

            try
            {
                List<Book> books = _bookService.GetAllBooksWithAuthor();
                if (books == null || books.Count == 0)
                {
                    Console.WriteLine("No books found in the database.");
                    return;
                }
                foreach (Book book in books)
                {
                    string authorName = book.Author != null ? book.Author.Name : "No Author";

                    Console.WriteLine($"ID: {book.Id} | Name: {book.Name} | Pages: {book.PageCount} | Author: {authorName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service Error: {ex.Message}");
            }
        }
        public void CreateAuthor()
        {
            while (true)
            {
                Console.Write("Enter Author Name (or 'menu' to exit): ");
                string name = Console.ReadLine()?.Trim();

                if (name?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Error: Author name cannot be empty! Try again.\n");
                    continue;
                }

                Console.Write("Enter Author Surname (Press Enter to skip): ");
                string surnameInput = Console.ReadLine()?.Trim();
                string surname = string.IsNullOrWhiteSpace(surnameInput) ? null : surnameInput;

                Console.WriteLine("Select Gender:");
                Console.WriteLine("1. Male");
                Console.WriteLine("2. Female");
                Console.Write("Your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int genderChoice) || (genderChoice != 1 && genderChoice != 2))
                {
                    Console.WriteLine("Error: Please select a valid gender option! Try again.\n");
                    continue;
                }
                Gender authorGender = (Gender)genderChoice;

                try
                {
                    Author newAuthor = new Author
                    {
                        Name = name,
                        Surname = surname,
                        Gender = authorGender
                    };

                    _authorService.CreateAuthor(newAuthor);

                    Console.WriteLine($"\nSuccess: Author '{newAuthor.Name}' created and added to database successfully!");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nService Error: {ex.Message} Try again.\n");
                }
            }
        }
        public void ShowAllAuthors()
        {
            Console.WriteLine("=== ALL AUTHORS LIST ===");

            try
            {
                var authors = _authorService.ShowAllAuthors();

                if (authors == null || authors.Count == 0)
                {
                    Console.WriteLine("No authors found in the database.");
                    return;
                }

                foreach (var author in authors)
                {
                    int bookCount = author.Books != null ? author.Books.Count : 0;
                    Console.WriteLine($"ID: {author.Id} | Name: {author.Name} | Total Books: {bookCount}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service Error: {ex.Message}");
            }
        }
        public void ShowAuthorsBooks()
        {
            while (true)
            {
                Console.Write("Enter Author ID (or 'menu' to exit): ");
                string input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!int.TryParse(input, out int authorId) || authorId <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid Author ID! Try again.\n");
                    continue;
                }

                try
                {

                    var author = _authorService.GetByAuthorId(authorId);

                    Console.WriteLine($"\n=== BOOKS BY AUTHOR: {author.Name} ===");

                    if (author.Books == null || author.Books.Count == 0)
                    {
                        Console.WriteLine("This author has no books in the database.");
                    }
                    else
                    {
                        foreach (var book in author.Books)
                        {
                            Console.WriteLine($"- Book ID: {book.Id} | Name: {book.Name} | Pages: {book.PageCount}");
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nService Error: {ex.Message} Try again.\n");
                }
            }
        }
        public void ReserveBook()
        {
            while (true)
            {
                Console.Write("Enter Book ID to reserve (or 'menu' to exit): ");
                string bookInput = Console.ReadLine()?.Trim();

                if (bookInput?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!int.TryParse(bookInput, out int bookId) || bookId <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid book ID! Try again.\n");
                    continue;
                }
                Console.Write("Enter your FinCode: ");
                string finCode = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(finCode))
                {
                    Console.WriteLine("Error: FinCode cannot be empty! Try again.\n");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(finCode) || finCode.Length != 7)
                {
                    Console.WriteLine("Error: Invalid FinCode! FinCode must be exactly 7 characters long. Try again.\n");
                    continue;
                }
                var allReservations = _reservedService.GetAllReservationsOrderedByStatus();
                int activeReservationsCount = 0;
                foreach (var res in allReservations)
                {
                    if (res.FinCode == finCode && res.Status.ToString() == "Active")
                    {
                        activeReservationsCount++;
                    }
                }
                if (activeReservationsCount >= 3)
                {
                    Console.WriteLine($"Error: FinCode '{finCode}' already has {activeReservationsCount} active reservations. Maximum allowed is 3!\n");
                    continue;
                }
                Console.Write("Enter Start Date:");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    Console.WriteLine("Error: Invalid Start Date format! Try again.\n");
                    continue;
                }
                if (startDate.Date < DateTime.Today)
                {
                    Console.WriteLine("Error: Start Date cannot be in the past! Try again.\n");
                    continue;
                }
                Console.Write("Enter End Date:");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    Console.WriteLine("Error: Invalid End Date format! Try again.\n");
                    continue;
                }
                if (endDate.Date <= startDate.Date)
                {
                    Console.WriteLine("Error: End Date must be after the Start Date! Try again.\n");
                    continue;
                }
                try
                {
                    ReservedItem newReservation = new ReservedItem
                    {
                        BookId = bookId,
                        FinCode = finCode,
                        StartDate = startDate,
                        EndDate = endDate,
                        Status = Status.Started
                    };
                    _reservedService.ReserveBook(newReservation);

                    Console.WriteLine($"\nSuccess: Book with ID {bookId} has been successfully reserved for FIN {finCode}!");
                    break;
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"\nService Error: {ex.Message} Try again.\n");
                }
            }
        }
        public void ShowReservationList()
        {
            Console.WriteLine("=== RESERVATION LIST (ORDERED BY STATUS) ===");

            try
            {
                var reservations = _reservedService.GetAllReservationsOrderedByStatus();

                if (reservations == null || reservations.Count == 0)
                {
                    Console.WriteLine("No reservations found in the database.");
                    return;
                }
                foreach (var res in reservations)
                {
                    Console.WriteLine($"Book ID: {res.BookId} | FIN: {res.FinCode} | Status: {res.Status} | From: {res.StartDate.ToShortDateString()} To: {res.EndDate.ToShortDateString()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service Error: {ex.Message}");
            }
        }
        public void ChangeReservationStatus()
        {
            while (true)
            { 
                Console.Write("Enter Reservation ID to change status (or 'menu' to exit): ");
                string input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "menu")
                {
                    Console.WriteLine("Returning to menu...");
                    return;
                }

                if (!int.TryParse(input, out int reservationId) || reservationId <= 0)
                {
                    Console.WriteLine("Error: Please enter a valid Reservation ID! Try again.\n");
                    continue;
                }

                try
                {
                    var currentReservation = _reservedService.GetReservationById(reservationId);

                    Console.WriteLine($"\nCurrent Status of this reservation: {currentReservation.Status}");

                    if (currentReservation.Status == Status.Completed || currentReservation.Status == Status.Canceled)
                    {
                        Console.WriteLine($"Error: This reservation is already {currentReservation.Status}. Finished reservations cannot be modified!\n");
                        return; 
                    }

                    Console.WriteLine("\nSelect New Status:");
                    Console.WriteLine("1. Confirmed");
                    Console.WriteLine("2. Started");
                    Console.WriteLine("3. Completed (Book Returned)");
                    Console.WriteLine("4. Canceled (Order Cancelled)");
                    Console.Write("Your choice: ");

                    string statusChoice = Console.ReadLine()?.Trim();
                    Status newStatus;

                    if (statusChoice == "1") newStatus = Status.Confirmed;
                    else if (statusChoice == "2") newStatus = Status.Started;
                    else if (statusChoice == "3") newStatus = Status.Completed;
                    else if (statusChoice == "4") newStatus = Status.Canceled;
                    else
                    {
                        Console.WriteLine("Error: Invalid status choice! Try again.\n");
                        continue;
                    }

                    if (currentReservation.Status == Status.Confirmed && newStatus == Status.Completed)
                    {
                        Console.WriteLine("Error: A 'Confirmed' reservation must be 'Started' before it can be 'Completed'!\n");
                        continue;
                    }
                    _reservedService.ChangeReservationStatus(reservationId, newStatus);

                    Console.WriteLine($"\nSuccess: Reservation ID {reservationId} status changed to {newStatus} successfully!");

                    if (newStatus == Status.Completed || newStatus == Status.Canceled)
                    {
                        Console.WriteLine("Info: The book has been successfully returned to the system log. User's limit is cleared.");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nService Error: {ex.Message} Try again.\n");
                }
            }
        }
        public void ShowUserReservations()
        {
            while (true)
            {
                Console.Write("Enter FinCode to view reservations (7 characters, or 'menu' to exit): ");
                string finCode = Console.ReadLine()?.Trim().ToUpper();

                if (finCode == "MENU") return;

                if (string.IsNullOrWhiteSpace(finCode) || finCode.Length != 7)
                {
                    Console.WriteLine("Error: FinCode must be exactly 7 characters long! Try again.\n");
                    continue;
                }

                try
                {
                    var reservations = _reservedService.GetReservationsByFinCode(finCode);

                    Console.WriteLine($"\n=== RESERVATION LIST FOR FIN: {finCode} ===");

                    if (reservations == null || reservations.Count == 0)
                    {
                        Console.WriteLine("No reservations found for this FinCode.");
                    }
                    else
                    {
                        foreach (var res in reservations)
                        {
                            Console.WriteLine($"Reservation ID: {res.Id} | Book ID: {res.BookId} | Status: {res.Status} | {res.StartDate.ToShortDateString()} - {res.EndDate.ToShortDateString()}");
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nService Error: {ex.Message} Try again.\n");
                }
            }
        }
    }
}





