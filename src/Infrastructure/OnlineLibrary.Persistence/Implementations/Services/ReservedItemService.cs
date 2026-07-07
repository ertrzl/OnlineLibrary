using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Domain.Enums;

namespace OnlineLibrary.Persistence.Implementations.Services
{
    public class ReservedItemService : IReservedItemService
    {
        private readonly IReservedItemRepository _reservedItems;
        private readonly IBookRepository _bookRepository;

        public ReservedItemService(IReservedItemRepository reservedItems, IBookRepository bookRepository)
        {
            _reservedItems = reservedItems;
            _bookRepository = bookRepository;
        }

        public void ChangeReservationStatus(int id, Status newStatus)
        {
            var reservation = _reservedItems.GetById(id, isTracking: true);
            if (reservation is null)
                throw new Exception($"Reservation with ID {id} not found!");
            if (reservation.Status == Status.Completed)
                throw new Exception("Error: Cannot change status of a completed reservation!");
            if (reservation.Status == Status.Canceled)
                throw new Exception("Error: Cannot change status of a canceled reservation!");
            if (reservation.Status == Status.Confirmed && newStatus == Status.Completed)
                throw new Exception("Error: A 'Confirmed' reservation must be 'Started' before it can be 'Completed'!");
            if (reservation.Status == Status.Started && newStatus == Status.Confirmed)
                throw new Exception("Error: A 'Started' reservation cannot go back to 'Confirmed'!");
            if (reservation.Status == newStatus)
                throw new Exception($"Error: Reservation is already '{newStatus}'. No change made!");

            reservation.Status = newStatus;
            _reservedItems.Update(reservation);
            _reservedItems.SaveChanges();
        }

        public List<ReservedItem> GetAllReservationsOrderedByStatus()
        {
            return _reservedItems.GetAll()
            .OrderBy(r => r.Status)
            .ToList();
        }
        public ReservedItem GetReservationById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.");
            }
            var reservation = _reservedItems.GetById(id);

            if (reservation == null)
            {
                throw new Exception($"Reservation with ID {id} does not exist in the database.");
            }
            return reservation;
        }


        public List<ReservedItem> GetReservationsByFinCode(string finCode)
        {
            if (string.IsNullOrWhiteSpace(finCode) || finCode.Length != 7)
                throw new Exception("FinCode must be exactly 7 characters long.");

            var results = _reservedItems.GetAll()
                .Where(r => r.FinCode == finCode)
                .ToList();


            if (results.Count == 0)
                throw new Exception($"Error: No reservations found for FIN code: {finCode}");

            return results;
        }

        public void ReserveBook(ReservedItem item)
        {
            if (item is null)
                throw new Exception("Error: Reservation item cannot be null!");
            if (string.IsNullOrWhiteSpace(item.FinCode))
                throw new Exception("Error: FIN code cannot be empty!");
            if (item.FinCode.Length != 7)
                throw new Exception("Error: FIN code must be exactly 7 characters long.");
            if (item.StartDate < DateTime.Now.Date)
                throw new Exception("Error: Start date cannot be in the past!");
            if (item.EndDate <= item.StartDate)
                throw new Exception("Error: End date must be after the start date!");
            if (item.Status == Status.Completed)
                throw new InvalidOperationException("Cannot modify a reservation that is already completed.");

            var book = _bookRepository.GetById(item.BookId);
            if (book is null)
                throw new Exception($"Error: Book with ID {item.BookId} does not exist!");
            var isBookAvailable = !_reservedItems.GetAll()
                .Any(r => r.BookId == item.BookId
              && (r.Status == Status.Confirmed || r.Status == Status.Started)
              && r.StartDate < item.EndDate
              && r.EndDate > item.StartDate);

            if (!isBookAvailable)
                throw new Exception($"Error: Book with ID {item.BookId} is already reserved for the selected dates!");

            var activeReservationsCount = _reservedItems.GetAll()
        .Count(r => r.FinCode == item.FinCode && (r.Status == Status.Confirmed || r.Status == Status.Started));
            if (activeReservationsCount >= 3)
                throw new Exception("Error: User with this FIN code already has 3 active reservations!");
            item.Status = Status.Confirmed;

            _reservedItems.Add(item);
            _reservedItems.SaveChanges();
        }
    }

}
