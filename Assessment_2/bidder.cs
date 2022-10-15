using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Assessment_2
{
    internal class bidder : Client
    {
        string user;
        string product;
        string price;
        string delivery;
        public bidder(string user_ID, string product_ID, string item_price, string delivery)
        {
            user = user_ID;
            product = product_ID;
            price = item_price;
            this.delivery = delivery;
        }

        public void save_details()
        {
            if (File.Exists("saved_bids.txt"))
            {
                string details = File.ReadAllText("saved_bids.txt");
                MatchCollection prev_num = Regex.Matches(details, "[0-9]+.\\r?\\nproduct ID: ");
                string this_num = (prev_num.Count + 1).ToString();

                using (StreamWriter sw = File.AppendText("saved_bids.txt"))
                {
                    sw.WriteLine(this_num + "." + "\nproduct ID: " + product + "\nuser ID: " + user + "\nbid price: " + price + "\ndelivery: " + this_num + delivery);
                }
            }
            else
            {
                File.WriteAllText("saved_bids.txt", "1.\nproduct ID: " + product + "\nuser ID: " + user + "\nbid price: " + price + "\ndelivery: 1" + delivery);
            }
        }
    }
}
