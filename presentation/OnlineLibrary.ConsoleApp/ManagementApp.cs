using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Persistence.DAL;
using OnlineLibrary.Persistence.Implementations.Repositories;
using OnlineLibrary.Persistence.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.ConsoleApp
{
    public class ManagementApp
    {
        private readonly ManageMethod _manage;

        public ManagementApp()
        {
            var db = new AppDbContext();
            var authorRepo = new AuthorRepository(db);
            var bookRepo = new BookRepository(db);
            var resRepo = new ReservedItemRepository(db);
            IAuthorService authorService = new AuthorService(authorRepo);
            IBookService bookService = new BookService(bookRepo, resRepo, authorRepo);
            IReservedItemService reserveService = new ReservedItemService(resRepo, bookRepo);

            _manage = new ManageMethod(authorService, bookService, reserveService);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("\x1b[3J");
                Console.WriteLine("=== ONLINE LIBRARY MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Create Book");
                Console.WriteLine("2. Delete Book");
                Console.WriteLine("3. Get Book By Id");
                Console.WriteLine("4. Show All Books");
                Console.WriteLine("5. Create Author");
                Console.WriteLine("6. Show All Authors");
                Console.WriteLine("7. Show Author's Books");
                Console.WriteLine("8. Reserve Book");
                Console.WriteLine("9. Reservation List");
                Console.WriteLine("10. Change Reservation Status");
                Console.WriteLine("11. Show User Reservations");
                Console.WriteLine("12. Filter Authors by Gender");

                Console.WriteLine("0. Exit");
                
                Console.Write("\nSelect an option: ");
                string choice = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(choice))
                    continue;

                switch (choice)
                {
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Exiting application...");
                        return;
                    case "1":
                        Console.Clear();
                        _manage.CreateBook();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "2":
                        Console.Clear();
                        _manage.DeleteBook(); 
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "3":
                        Console.Clear();
                        _manage.GetBookById();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "4":
                        Console.Clear();
                        _manage.ShowAllBooks();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "5":
                        Console.Clear();
                        _manage.CreateAuthor(); 
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "6":
                        Console.Clear();
                        _manage.ShowAllAuthors();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "7":
                        Console.Clear();
                        _manage.ShowAuthorsBooks(); 
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "8":
                        Console.Clear();
                        _manage.ReserveBook(); 
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "9":
                        Console.Clear();
                        _manage.ShowReservationList();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "10":
                        Console.Clear();
                        _manage.ChangeReservationStatus(); 
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "11":
                        Console.Clear();
                        _manage.ShowUserReservations();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;
                    case "12":
                        Console.Clear();
                        _manage.FilterAuthorsByGender();
                        Console.WriteLine("\nPress Enter to return to main menu...");
                        Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("Invalid option! Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}

    

