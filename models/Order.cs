using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHackTeam5.models
{
    public class Header
    {
        public string salesNumber { get; set; }
        public string dateTime { get; set; }
        public string locationId { get; set; }
        public string locationName { get; set; }
        public string locationAddress { get; set; }
        public string locationPostcode { get; set; }
        public string totalCost { get; set; }
        public string totalTax { get; set; }
        public string receiptUrl { get; set; }
    }

    public class DetailsItem
    {
        public string productId { get; set; }
        public string quantity { get; set; }
        public string unitCost { get; set; }
        public string totalCost { get; set; }
        public string totalTax { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
    }

    public class Order
    {
        public string id { get; set; }
        public Header header { get; set; }
        public List<DetailsItem> details { get; set; }
    }

}
