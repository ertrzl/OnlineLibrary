using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entitites;
using OnlineLibrary.Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace OnlineLibrary.Persistence.Implementations.Repositories
{
    public class ReservedItemRepository : GenericRepository<ReservedItem>, IReservedItemRepository
    {
        public ReservedItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
