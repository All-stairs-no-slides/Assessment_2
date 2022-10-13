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
            MatchCollection items = Regex.Matches(details, "user ID: " + this.client_num + "\r?\nproduct name: .+\r?\nproduct price: .+\r?\nproduct description: .+\r?\n");
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
                    Console.WriteLine(item_num.ToString() + "    " + final_prod_name + "    " + final_prod_desc + "    " + final_prod_price);
                }
            }
        }
    }
}
