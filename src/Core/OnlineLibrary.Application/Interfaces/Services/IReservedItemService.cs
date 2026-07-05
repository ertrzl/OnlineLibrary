using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Application.Interfaces.Services
{
    public interface IReservedItemService
    {
        void ReserveBook(ReservedItem item);
        List<ReservedItem> GetAllReservationsOrderedByStatus();
        void ChangeReservationStatus(int id, Status newStatus);
        List<ReservedItem> GetReservationsByFinCode(string finCode);

        ReservedItem GetReservationById(int id);


    }
}
