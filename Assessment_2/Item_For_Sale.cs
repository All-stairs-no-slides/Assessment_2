using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_2
{
    internal class Item_For_Sale
    {
        public string client_ID;
        public string name;
        public string description;
        public string price;

        // constructor
        public Item_For_Sale(string name, string description, string price, string client_ID)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.client_ID = client_ID;
        }

        public void save_Item()
        {
            if (File.Exists("Saved_Products.txt"))
            {
                using (StreamWriter sw = File.AppendText("Saved_Products.txt"))
                {
                    sw.WriteLine("\nuser ID: " + client_ID +  "\nproduct name: " + name + "\nproduct price: " + price + "\nproduct description: " + description);
                }
            }
            else
            {
                File.WriteAllText("Saved_Products.txt", "\nuser ID: " + client_ID + "product name: " + name + "\nproduct price: " + price + "\nproduct description: " + description);
            }
        }
    }
}
