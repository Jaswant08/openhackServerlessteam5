using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHackTeam5.models
{
    public class Header
    {
        public string SalesNumber { get; set; }
        public string DateTime { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationPostcode { get; set; }
        public string TotalCost { get; set; }
        public string TotalTax { get; set; }
        public string ReceiptUrl { get; set; }
    }

    public class DetailsItem
    {
        public string ProductId { get; set; }
        public string Quantity { get; set; }
        public string UnitCost { get; set; }
        public string TotalCost { get; set; }
        public string TotalTax { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        public Header Header { get; set; }
        public List<DetailsItem> Details { get; set; }
    }

}
