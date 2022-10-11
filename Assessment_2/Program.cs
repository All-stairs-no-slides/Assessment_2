using System.Text.RegularExpressions;
using System.IO;

namespace Assessment_2
{
    internal class Program
    {
        static Client logged_in_Client = new Client();
        public static void Save_Details(string saved_names, string saved_passwords, string saved_emails)
        {
            if (File.Exists("saved_Clients.txt"))
            {
                using (StreamWriter sw = File.AppendText("saved_Clients.txt"))
                {
                    sw.WriteLine("name: " + saved_names + "\nemail: " + saved_emails + "\npassword: " + saved_passwords);
                }
            }
            else
            {
                File.WriteAllText("saved_Clients.txt", "name: " + saved_names + "\nemail: " + saved_emails + "\npassword: " + saved_passwords + "\n");
            }
        }
        private static void login()
        {
            string email;
            string password;
            string details = File.ReadAllText("saved_Clients.txt");
            Console.WriteLine("\nSign In \n-------\n");
            string identifier;
            while (true)
            {
                Console.WriteLine("Please Enter Your Email Address");
                email = Console.ReadLine();
                string email_regex = "email: " + email + "\n";
                Match found_email = System.Text.RegularExpressions.Regex.Match(details, email_regex);
                if (found_email.Success)
                {
                    identifier = email;
                    break;
                }
                Console.WriteLine("\nThere was no " + email_regex);
            }
            logged_in_Client.email = email;
            while (true)
            {
                Console.WriteLine("\nPlease Enter Your Password");
                password = Console.ReadLine();
                string password_regex = identifier + "\npassword: " + password + "\n";
                Match found_password = System.Text.RegularExpressions.Regex.Match(details, password_regex);
                if (found_password.Success)
                {
                    break;
                }
                Console.WriteLine("\nThere was no " + password_regex);
            }
            logged_in_Client.password = password;

            Match Clientnum = System.Text.RegularExpressions.Regex.Match(details, "[0-9]+.+\n.+\nemail: " + identifier);
            Clientnum = Regex.Match(Clientnum.Value, "[0-9]+");

            // do the address input loop here
            Match does_address_exist = System.Text.RegularExpressions.Regex.Match(details, Clientnum.Value + " has registered address: ");
            if (!does_address_exist.Success)
            {
                int unit_num;
                int street_num;
                string street_name;
                string street_suffix;
                string city;
                string state;
                int postcode;
                Console.WriteLine("\nPlease Provide Your Home Address");
                while (true)
                {
                    Console.WriteLine("\nUnit number (0 = none):");
                    string inputted_unit_num = Console.ReadLine();
                    bool success = Int32.TryParse(inputted_unit_num, out unit_num);
                    if (success && unit_num >= 0)
                    {
                        break;
                    }
                    Console.WriteLine("Unit number must be a non-negative integer");
                }
                while (true)
                {
                    Console.WriteLine("\nStreet number:");
                    string inputted_street_num = Console.ReadLine();
                    bool success = Int32.TryParse(inputted_street_num, out street_num);
                    if (!success)
                    {
                        Console.WriteLine("Street number must be a positive integer");
                    } else if (street_num <= 0)
                    {
                        Console.WriteLine("Street number must be greater than zero");
                    }
                    else
                    {
                        break;
                    }
                }
                Console.WriteLine("\nStreet name:");
                street_name = Console.ReadLine();
                Console.WriteLine("\nStreet suffix:");
                street_suffix = Console.ReadLine();
                Console.WriteLine("\nCity:");
                city = Console.ReadLine();
                while (true)
                {
                    Console.WriteLine("\nState (ACT, NSW, NT, SA, QLD, TAS, VIC, WA):");
                    state = Console.ReadLine();
                    if (state == "ACT"|| state == "NSW"|| state == "NT" || state == "SA"|| state == "QLD"||state == "TAS"||state == "VIC"|| state == "WA"||
                        state == "act" || state == "nsw" || state == "nt" || state == "sa" || state == "qld" || state == "tas" || state == "vic" || state == "wa")
                    {
                        break;
                    }
                    Console.WriteLine("state must be either: ACT, NSW, NT, SA, QLD, TAS, VIC or WA");
                }
                while (true)
                {
                    Console.WriteLine("\nPostcode (1000 .. 9999)");
                    string input_postcode = Console.ReadLine();
                    bool success = Int32.TryParse(input_postcode, out postcode);
                    if (success && postcode > 100 && postcode < 9999)
                    {
                        break;
                    } 
                }
                string final_address = unit_num.ToString() + " " + street_num.ToString() + " " + street_name + " " + street_suffix + " " + city + " " + state + " " + postcode;
                using (StreamWriter sw = File.AppendText("saved_Clients.txt"))
                {
                    sw.WriteLine(Clientnum.Value + " has registered address: " + final_address);
                }
                if(unit_num == 0)
                    Console.WriteLine("Address have been updated to " + street_num.ToString() + " " + street_name + " " + street_suffix + ". " + city + " " + state + " " + postcode);
                else
                {
                    Console.WriteLine("Address have been updated to " + unit_num.ToString() + " " + street_num.ToString() + " " + street_name + " " + street_suffix + ". " + city + " " + state + " " + postcode);
                }
            }
        }
        static void Main(string[] args)
        {
            int state = 1;
            bool running = true;
            while (running)
            {
                switch (state)
                {
                    case 1:
                        Console.WriteLine("+--------------------------------+");
                        Console.WriteLine("|  Welcome To the Auction House  |");
                        Console.WriteLine("+--------------------------------+");
                        Console.WriteLine("\n");
                        Console.WriteLine("Main Menu \n--------- \n(1) Register \n(2) Sign In \n(3) Exit");
                        Console.WriteLine("\nPlease Select a Number Between 1 and 3");
                        string input = Console.ReadLine();
                        switch (input)
                        {
                            case "1": // initial sign/register select in page
                                //TODO(for future me) you only need 2 call the class locally and then you can just store the variables in a file(so u dont need 2 keep multiple objects for each individual user)
                                Console.WriteLine("");
                                Client new_user = new Client();
                                new_user.Register();
                                Save_Details(new_user.name, new_user.password, new_user.email);
                                input = null;
                                state = 1;
                                break;
                            case "2":
                                login();
                                state = 2;
                                input = null;
                                break;
                            case "3":
                                running = false;
                                input = null;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2: // Client Menu page
                        Console.WriteLine("\nClient Menu------\n(1) Advertise Product\n(2) View My Poroduct list\n (3) Search For Advertised Products\n(4) View Bids On My Products\n(5) View My Purchased Itedms\n(6) Log off\n\nPlease select an option between 1 and 6");
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "1":
                                input = null;
                                logged_in_Client.Advertise_Product();
                                break;
                            case "2":
                                input = null;
                                break;
                            case "3":
                                input = null;
                                break;
                            case "4":
                                input = null;
                                break;
                            case "5":
                                input = null;
                                break;
                            case "6":
                                input = null;
                                break;
                            case "7":
                                input = null;
                                break;
                            case "8":
                                input = null;
                                break;
                        }
                        break;
                }
            }
        }
    }
}