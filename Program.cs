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
    ///  - Hanro Lombard (603200) - Did most to all of the code
    ///  - Thabo Hammer (603918) - Made small changes to the code
    ///  - Tarah Barwe () - Made small changes to the code
    /// </summary>
    /// 

    /// <summary>
    /// ==== MILESTONE 2 ====
    /// Contributions
    ///  - Hanro Lombard (603200)
    ///  - [No other contributions]
    /// </summary>

    internal class Program
    {
        // Problem solved: All of the BookingManager methods were static,
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
                // used ToString() - ambiguous
                write.print($"[{(int)option}] - {option}\n");
            }
            write.print(new string('=', 40) + "\n");
        }



        static void Main(string[] args)
        {
            List<Booking> bookings = new List<Booking>();

            try
            {
                // Pass the list to AddPredefinedBookings so they are captured
                manager.AddPredefinedBookings(bookings);

            } catch(Exception e)
            {
                write.print($"Error loading predefined bookings {e}");
            }
            while (true)
            {
                DisplayMenu();
                write.print("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    write.print("Invalid input. Please enter a number.\n");
                    continue;
                }

                try
                {
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

                        case 6:
                            // Clear the console for spacing / clarity
                            Console.Clear();
                            write.print("Console cleared!");
                            break;
                        default:
                            write.print("Invalid option. Please choose an option from the list below\n");
                            break;
                    }
                } catch(InvalidOperationException ioe)
                {
                    write.print($"Exception Occurred: {ioe.Message}");
                } catch(Exception e)
                {
                    write.print($"Unexpected error: {e}");
                }
            }
        }
    }
}
