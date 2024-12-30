using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.BaseEntities
{
    public class MainEntity
    {
        public Guid Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte Status { get; set; } = 0;
    }
}
