using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Persistence.DAL;


namespace OnlineLibrary.Persistence.Implementations.Repositories
{
    public class ReservedItemRepository : GenericRepository<ReservedItem>, IReservedItemRepository
    {
        public ReservedItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
