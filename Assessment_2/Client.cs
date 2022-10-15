using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Assessment_2
{
    internal class Client
    {
        public string client_num;
        public string name;
        public string password;
        public string email;
        // put home address variables in here too

        private void check(int check_type) // check_type used to see whether it should check for email syntax of=r password syntax (name needs no syntax)
        {
            string email_params = "[A-Za-z0-9_.-]*[A-Za-z0-9]@[A-Za-z0-9-]+[A-Za-z0-9-.]*.[A-Za-z]+";
            string password_params = "([A-Z]{1,}|[a-z]{1,}|[0-9]{1,}|[!@#$%^&*]{1,}){8,}";
            switch (check_type)
            {
                case 1:
                    if (!System.Text.RegularExpressions.Regex.IsMatch(email ,email_params))
                    {
                        email = null;
                        Console.WriteLine("     The suplied value is not a valid email");
                    }
                    break;

                case 2:
                    if (!System.Text.RegularExpressions.Regex.IsMatch(password, password_params))
                    {
                        password = null;
                        Console.WriteLine("     The suplied value is not a valid password");
                    }
                    break;
                default:
                    if (name == "")
                    {
                        name = null;
                        Console.WriteLine("     Please enter a value");
                    }
                    break;
            }
        }

        public void Register()
        {
            Console.WriteLine("Registration\n------------"); // write the stuff here that needs to go be4 the registration happens
            while(name == null) {
                Console.WriteLine("\nPlease enter your name");
                name = Console.ReadLine();
                if (name == "")
                {
                    name = null;
                }
            }
            while(email == null)
            {
                Console.WriteLine("\nPlease enter email address");
                email = Console.ReadLine();
                check(1);
            }
            while(password == null)
            {
                Console.WriteLine("\nPlease choose a password" +
                    "\n* At least 8 characters\n* No white space characters" +
                    "\n* At least one upper-case letter" +
                    "\n* At least one lowe case letter" +
                    "\n* at least one digit" +
                    "\n* at least  one special character");
                
                password = Console.ReadLine();
                check(2);
            }
        }
        public void Advertise_Product()
        {
            Console.WriteLine("Product Advertisment\n------------");
            string prod_name;
            string Prod_Init_Price;
            string Prod_Description;
            while (true)
            {
                Console.WriteLine("\nProduct Name");
                prod_name = Console.ReadLine();
                if (Regex.IsMatch(prod_name, "[\x21-\x7E]"))
                {
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("\nInitial Price ($d.cc)");
                Prod_Init_Price = Console.ReadLine();
                if (Regex.IsMatch(Prod_Init_Price, "\\$[0-9]+\\.[0-9][0-9]")) // fix this later so that it actually checks only for a period
                {
                    break;
                }
                Console.WriteLine("a Currency is required e.g. '$12.00' or '$19.00'");
            }
            while (true)
            {
                Console.WriteLine("\nProduct Description");
                Prod_Description = Console.ReadLine();
                if (Regex.IsMatch(Prod_Description, "[\x21-\x7E]"))
                {
                    Item_For_Sale new_Item = new Item_For_Sale(prod_name, Prod_Description, Prod_Init_Price, client_num);
                    new_Item.save_Item();
                    break;
                }
            }
        }

        public void View_products()
        {
            Console.WriteLine("\nItem #    Product Name    Description    Listed price    Bidder name    Bidder email    Bid amt");
            string details = File.ReadAllText("Saved_Products.txt");
            string bid_doc = File.ReadAllText("saved_bids.txt");
            string clients = File.ReadAllText("saved_Clients.txt");
            MatchCollection items = Regex.Matches(details, "[0-9]+\\.\r?\nuser ID: " + client_num + "\r?\nproduct name: .+\r?\nproduct price: .+\r?\nproduct description: .+\r?\n");
            if(items.Count > 0)
            {
                int item_num = 0;
                foreach(Match item in items)
                {
                    item_num++;
                    Match prod_name = Regex.Match(item.Value, "(\\r?\\nproduct name: )(.+)\\r?\\n");
                    string final_prod_name = prod_name.Groups[2].Value.Replace("\r", "");
                    Match prod_desc = Regex.Match(item.Value, "(\\r?\\nproduct description: )(.+)");
                    string final_prod_desc = prod_desc.Groups[2].Value.Replace("\r", "");
                    Match prod_price = Regex.Match(item.Value, "(\\r?\\nproduct price: )(.+)\\r?\\n");
                    string final_prod_price = prod_price.Groups[2].Value.Replace("\r", "");
                    Match prod_num = Regex.Match(item.Value, "([0-9]+)\\.\\r?\\nuser ID: ");
                    MatchCollection bids = Regex.Matches(bid_doc, "product ID: " + prod_num.Groups[1].Value + "\r?\nuser ID: ([0-9]+)\r?\nbid price: \\$([0-9]+\\.[0-9][0-9])");
                    double highest_bid = 0.00;
                    Match client = Regex.Match("", "");
                    foreach(Match bid in bids)
                    {
                        double int_ver_bid = Double.Parse(bid.Groups[2].Value);
                        if (highest_bid < int_ver_bid)
                        {
                            highest_bid = int_ver_bid;
                            client = Regex.Match(clients, bid.Groups[1].Value + "\\.\r?\nname: (.+)\r?\nemail: (.+)\r?\n");
                        }
                    }
                    Console.WriteLine(item_num.ToString() + "    " + final_prod_name + "    " + final_prod_desc + "    " + final_prod_price + "    " + client.Groups[1].Value + "    " + client.Groups[2].Value + "    " +  highest_bid.ToString());
                }
            }
        }

        public void place_bid(string the_items, int num_of_items)
        {
            Console.WriteLine("\nPlease enter a non-negative integer between 1 and " + num_of_items.ToString());
            string bid_on_item = Console.ReadLine();
            int bid_on_num = Int32.Parse(bid_on_item);
            MatchCollection new_matches = Regex.Matches(the_items, "([0-9]).\\r?\\nuser ID: ([0-9]+)\\r?\\nproduct name: (.+)\\r?\\nproduct price: (\\$[0-9]+\\.[0-9][0-9])");
            double highest_bid = 0.00;
            if (File.Exists("saved_bids.txt"))
            {
                string bids = File.ReadAllText("saved_bids.txt");
                MatchCollection relevant_bid = Regex.Matches(bids, (new_matches[bid_on_num - 1].Groups[1].Value) + "\r?\nuser ID: ([0-9]+)\r?\nbid price: \\$([0-9]+\\.[0-9][0-9])");
                foreach (Match match in relevant_bid)
                {
                    double int_ver_bid = Double.Parse(match.Groups[2].Value);
                    if (highest_bid < int_ver_bid)
                    {
                        highest_bid = int_ver_bid;
                    }
                }
            }
            Console.WriteLine("Bidding for " + new_matches[bid_on_num - 1].Groups[3].Value + " (regular price " + new_matches[bid_on_num - 1].Groups[4].Value + "), current highest bid $" + highest_bid.ToString() + "\n\nHow much do you bid?");
            string new_bid = Console.ReadLine();
            string method;
            while (true)
            {
                Console.WriteLine("\nDelivery Instructions\n----------------\n(1) Click and Collect\n(2) Home Delivery");
                method = Console.ReadLine();
                if (method == "1" || method == "2")
                {
                    break;
                }
            }
            string delivery = "Home Delivery";
            if (method == "1") 
            { 
                DateTime start_window;
                DateTime end_window;
                while (true)
                {
                    Console.WriteLine("Delivery Window Start (DD/MM/YYYY HH:MM)");
                    string window_start = Console.ReadLine();
                    try
                    {
                        start_window = DateTime.Parse(window_start);
                    } catch {
                        Console.WriteLine("Please enter a valid date time");
                        continue;
                    }
                    if (DateTime.Now.AddHours(1) < start_window)
                    {
                        break;
                    }
                    Console.WriteLine("Delivery Window must start at least one hour in the future");
                }
                while (true)
                {
                    Console.WriteLine("Delivery Window End (DD/MM/YYYY HH:MM)");
                    string window_end = Console.ReadLine();
                    try
                    {
                        end_window = DateTime.Parse(window_end);
                    }
                    catch
                    {
                        Console.WriteLine("Please enter a valid date time");
                        continue;
                    }
                    if (start_window.AddHours(1) < end_window)
                    {
                        break;
                    }
                    Console.WriteLine("Delivery end Window must start at least one hour in the future of the delivery start window");
                }
                delivery = "Click_and_collect " + start_window.ToString() + " " + end_window.ToString();
            }
            
            bidder new_bidder = new bidder(client_num, new_matches[bid_on_num - 1].Groups[1].Value, new_bid, delivery);
            new_bidder.save_details();
        }

        public void view_bid_on_items()
        {
            // viewing bids on mine own products
            string all_products = File.ReadAllText("Saved_Products.txt");
            string all_bids = File.ReadAllText("Saved_bids.txt");
            MatchCollection my_prods = Regex.Matches(all_products, "([0-9]+)\\.\\r?\\nuser ID: " + client_num + "\\r?\\nproduct name: (.+)\\r?\\nproduct price: \\$([0-9]\\.[0-9][0-9])\\r?\\nproduct description: (.+)");
            string highest_bid_ID = "";
            double prev_highest = 0.00;
            int item_num = 0;
            string all_bid_IDs = "";
            Console.WriteLine("\nItem #    Product Name    Description    Listed price    Bidder name    Bidder email    Bid amt");
            foreach (Match match in my_prods)
            {
                MatchCollection bids = Regex.Matches(all_bids, "([0-9]+)\\.\\r?\\nproduct ID: " + match.Groups[1].Value + "\\r?\\nuser ID: ([0-9]+)\\r?\\nbid price: \\$([0-9]+\\.[0-9][0-9])");
                int bid_index = 0;
                int high_index = 0;
                foreach(Match bid in bids)
                {
                    bid_index++;
                    double new_bid = Double.Parse(bid.Groups[3].Value);
                    if (new_bid > prev_highest)
                    {
                        high_index = bid_index;
                        prev_highest = new_bid;
                        highest_bid_ID = bid.Groups[1].Value;
                    }
                }
                if(high_index == 0)
                {
                    continue;
                }
                item_num++;
                string the_clients = File.ReadAllText("saved_Clients.txt");

                all_bid_IDs += highest_bid_ID + ",";

                Match high_bid = Regex.Match(all_bids, highest_bid_ID + "\\.\\r?\\nproduct ID: (.+)\\r?\\nuser ID: ([0-9]+)\\r?\\nbid price: \\$([0-9]\\.[0-9][0-9])");

                Match high_bidder = Regex.Match(the_clients, high_bid.Groups[2].Value + "\\.\\r?\\nname: (.+)\\r?\\nemail: (.+)\\r?\\n");

                Match the_prod = Regex.Match(match.Value, high_bid.Groups[1].Value + "\\.\\r?\\nuser ID: [0-9]+\\r?\\nproduct name: (.+)\\r?\\nproduct price: \\$([0-9]\\.[0-9][0-9])\\r?\\nproduct description: (.+)");
                Console.WriteLine(item_num + "    "  + the_prod.Groups[1].Value.Replace("\r", "") + "    " + the_prod.Groups[3].Value.Replace("\r", "") + "    " + the_prod.Groups[2].Value.Replace("\r", "") + "    " + high_bidder.Groups[1].Value.Replace("\r", "") + "    " + high_bidder.Groups[2].Value.Replace("\r", "") + "    $" + high_bid.Groups[3].Value.Replace("\r", ""));

            }
            Console.WriteLine("\nwould you like to sell any products?");
            string yes_no = Console.ReadLine();
            if(yes_no == "yes" || yes_no == "no")
            {
                purchase(all_bid_IDs);
            }
        }

        void purchase(string all_bids) // all_bids is a collation of all of the listed bids ID's that where found in the method above. Future me: use these 2 relate 2 the num input that u will do below
        {
            MatchCollection each_bid = Regex.Matches(all_bids, "([0-9]+),"); // the bids as an array
            string products_doc = File.ReadAllText("Saved_Products.txt");
            string bids_doc = File.ReadAllText("Saved_bids.txt");
            int which_bid_int;
            while (true)
            {
                Console.WriteLine("\nPlease Select a number between 1 and " + each_bid.Count.ToString()); // prompt to find which bid to do
                string which_bid = Console.ReadLine();
                if (Int32.TryParse(which_bid, out which_bid_int))
                {
                    if(which_bid_int > each_bid.Count || which_bid_int < 1) // check if input was less than max amount and larger than 1
                    {
                        continue;
                    }
                    break;
                }
            }

            string bid_ID = each_bid[which_bid_int - 1].Value.Replace(",", "");
            Match this_bid = Regex.Match(bids_doc, bid_ID + "\\.\\r?\\nproduct ID: ([0-9]+)\\r?\\nuser ID: ([0-9]+)\\r?\\nbid price: \\$([0-9]\\.[0-9][0-9])");
            string product_ID = this_bid.Groups[1].Value;
            Match product = Regex.Match(products_doc, product_ID + "\\.\\r?\\nuser ID: ([0-9])+\\r?\\nproduct name: (.+)\\r?\\nproduct price: \\$([0-9]\\.[0-9][0-9])\\r?\\nproduct description: (.+)");

            bids_doc = bids_doc.Replace(this_bid.Value, "");
            products_doc = products_doc.Replace(product.Value, "");

            if (File.Exists("saved_purchases.txt"))
            {
                string details = File.ReadAllText("saved_purchases.txt");
                MatchCollection prev_num = Regex.Matches(details, "([0-9]+)\\.");
                string this_num = (Int32.Parse(prev_num[prev_num.Count - 1].Groups[1].Value) + 1).ToString();
                using (StreamWriter sw = File.AppendText("saved_purchases.txt"))
                {
                    sw.WriteLine(this_num + ".\nselling user: " + product.Groups[1].Value + "\nbuying user: " + this_bid.Groups[2].Value + "\nproduct name: " + product.Groups[2].Value + "\nproduct price: " + product.Groups[3].Value + "\nbidding ID: " + bid_ID + "\nproduct description: " + product.Groups[4].Value + "\namt paid: " + this_bid.Groups[3].Value);
                }
            }
            else
            {
                File.WriteAllText("saved_purchases.txt", "1.\nselling user: " + client_num + "\nbuying user: " + this_bid.Groups[2].Value + "\nproduct name: " + product.Groups[2].Value + "\nproduct price: " + product.Groups[3].Value + "\nbidding ID: " + bid_ID + "\nproduct description: " + product.Groups[4].Value + "\namt paid: " + this_bid.Groups[3].Value);
            }

            if (Regex.Match(bids_doc, ".").Success)
            {
                File.WriteAllText("saved_bids.txt", bids_doc);
            }
            else
            {
                File.Delete("saved_bids.txt");
            }

            if (Regex.Match(products_doc, ".").Success)
            {
                File.WriteAllText("saved_products.txt", products_doc);
            } else
            {
                File.Delete("saved_products.txt");
            }
        }
        public void view_purchase_items()
        {
            string all_purchased_products = File.ReadAllText("saved_purchases.txt");
            string all_clients = File.ReadAllText("saved_Clients.txt");
            string bids = File.ReadAllText("saved_bids.txt");
            MatchCollection purchased_products = Regex.Matches(all_purchased_products, "([0-9])+\\.\\r?\\nselling user: ([0-9]+)\\r?\\nbuying user: " + client_num + "\\r?\\nproduct name: (.+)\\r?\\nproduct price: ([0-9]\\.[0-9][0-9])\\r?\\nbidding ID: ([0-9]+)\\r?\\nproduct description: (.+)\\r?\\namt paid: ([0-9]\\.[0-9][0-9])");
            Console.WriteLine("Item#    Seller email    Product name    Description    Listed price    Amt paid    Delivery option");
            int item_num = 0;
            
            foreach(Match match in purchased_products)
            {
                Match seller_email = Regex.Match(all_clients, match.Groups[1].Value + "\\.\\r?\\nname: (.+)\\r?\\nemail: (.+)\\r?\\n");
                Match option = Regex.Match(bids, "delivery: " + match.Groups[5].Value + "(.+)");
                item_num++;
                Console.WriteLine(item_num.ToString() + "    " + seller_email.Groups[2].Value + "    " + match.Groups[3].Value + "    " + match.Groups[6].Value + "    $" + match.Groups[4].Value + "    $" + match.Groups[7].Value + "    " + option.Groups[1].Value);
            }
        }
    }
}
