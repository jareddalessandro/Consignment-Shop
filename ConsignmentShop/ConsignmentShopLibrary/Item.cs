using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsignmentShopLibrary
{
    public class Item
    {
        public string Package { get; set; }
        public string Sheets  { get; set; }
        public decimal Price { get; set; }        
        public decimal Quantity { get; set; }
        public bool PaymentDistributed { get; set; }
        public Vendor Owner { get; set; }

        public string Display
        {
            get
            {
                return string.Format("{0} - ${1}     ({2}) in stock", Package, Price, Quantity);
            }
        }
    }
}
