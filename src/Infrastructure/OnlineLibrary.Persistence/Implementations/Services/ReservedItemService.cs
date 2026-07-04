using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Persistence.Implementations.Services
{
    internal class ReservedItemService : IReservedItemService
    {
        private readonly IReservedItemRepository _reservedItems;

        public ReservedItemService(IReservedItemRepository reservedItems)
        {
            _reservedItems = reservedItems;
        }

        public void ChangeReservationStatus(int id, Status newStatus)
        {
            var reservation = _reservedItems.GetById(id);
            if (reservation is null)
                throw new Exception($"Reservation with ID {id} not found!");

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

        public List<ReservedItem> GetReservationsByFinCode(string finCode)
        {
            if (string.IsNullOrWhiteSpace(finCode))
                throw new Exception("Error: FIN code cannot be empty!");

            var results = _reservedItems.GetAll()
            .Where(r => r.FinCode == finCode)
            .ToList();

            if (results.Count == 0)
                throw new Exception($"Error: No reservations found for FIN code: {finCode}");

            return results;
        }

        public void ReserveBook(ReservedItem item)
        {
            throw new NotImplementedException();
        }
    }
}
