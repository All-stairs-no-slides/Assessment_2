using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_2
{
    internal class Item_For_Sale
    {
        public string name;
        public string description;
        public string price;

        // constructor
        public Item_For_Sale(string name, string description, string price)
        {
            this.name = name;
            this.description = description;
            this.price = price;
        }
    }
}
