using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeCommand.Data.Models
{
    public class OrderProducts
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        // Additional properties related to the relationship
        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
