using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHackTeam5.models
{
    public class Receipt
    {
        public int totalItems { get; set; }
        public decimal totalCost { get; set; }
        public string salesNumber { get; set; }
        public string salesDate { get; set; }
        public string storeLocation { get; set; }
        public string receiptUrl { get; set; }
    }

}
