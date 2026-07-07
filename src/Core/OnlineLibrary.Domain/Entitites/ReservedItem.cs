using OnlineLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Entitites
{
    public class ReservedItem:BaseEntity
    {
        public string FinCode { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
    }
}
