using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRG261_TechnoLab
{

    /// <summary>
    /// Contributors
    /// Hanro Lombard (603200) - Did most to all of the code
    /// Thabo Hammer (603918)- Made small changes to the code
    /// Tarah Barwe () - Made small changes to the code
    /// </summary>
    
    internal class Program
    {
        // Problem solved: All of the BOokigManager methods were static,
        // causing it to error out when calling manager (instantiated)
        public static BookingManager manager = new BookingManager();
        public static Write write = new Write();
       
        
        enum MenuOptions
        {
            CaptureBookingRequests = 1,
            EvaluateBookingElgibility,
            DisplayBookingStats,
            Exit
        }

        public static void DisplayMenu()
        {
            write.print("\n" + new string('=', 40) + "\n");
            write.print("     TechnoLab Booking System\n");
            write.print(new string('=', 40) + "\n");
            foreach (var option in Enum.GetValues(typeof(MenuOptions)))
            {
                write.print($"{(int)option} - {option.ToString()}\n");
            }
            write.print(new string('=', 40) + "\n");
        }

        

        static void Main(string[] args)
        {
            List<Booking> bookings = new List<Booking>();
            
            // Pass the list to AddPredefinedBookings so they are captured
            manager.AddPredefinedBookings(bookings);

            while (true)
            {
                DisplayMenu();
                write.print("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    write.print("Invalid input. Please enter a number.\n");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        bool more = true;
                        while (more)
                        {
                            bookings = manager.CaptureBookingRequests(bookings);
                            write.print("Add another (y/n)? ");
                            string res = Console.ReadLine().ToLower();
                            if (string.IsNullOrEmpty(res) || res[0] == 'n') more = false;
                        }
                        break;
                    case 2:
                        manager.EvaluateBookingElgibility(bookings);
                        break;
                    case 3:
                        manager.DisplayBookingStats();
                        break;
                    case 4:
                        write.print("\nGoodbye!\n");
                        return;
                    case 5:
                        manager.VerifyBooking(bookings);
                        break;
                    default:
                        write.print("Invalid option. Please choose an option from the list below\n");
                        break;
                }
            }
        }
    }
}
