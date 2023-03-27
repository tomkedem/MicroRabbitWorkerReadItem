using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domian.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; } 
        // Other properties
        public int UserId { get; set; }
        public User User { get; set; }
    }
    

}
