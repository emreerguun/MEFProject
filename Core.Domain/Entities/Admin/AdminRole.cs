using Core.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities.Admin
{
    public partial class AdminRole : MainEntity
    {
        [Required]
        public string Title { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; } 
        public bool Delete { get; set; }
    }
}
