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
    public class Booking
    {
        public string StudentfName;
        public int bookingDuration;
        public int studentNumber;
        public string ContactNumber;
        public string StudentlName;
        public int YearOfStudy;
        public string equipType;
        public bool hasCompletedRequiredTraining;

        public Booking(string studentName, int bookingDuration, int studentNum, string contactNumber, string lname, int YearOfStudy, string equipType, bool hasCompletedRequiredTraining)
        {
            this.StudentfName = studentName;
            this.bookingDuration = bookingDuration;
            this.studentNumber = studentNum;
            this.ContactNumber = contactNumber;
            this.StudentlName = lname;
            this.YearOfStudy = YearOfStudy;
            this.equipType = equipType;
            this.hasCompletedRequiredTraining = hasCompletedRequiredTraining;
        }
    }

    internal class Program
    {
        // These lists store the results of the evaluation process
        public static List<Booking> ApprovedBookings = new List<Booking>();
        public static List<Booking> RejectedBookings = new List<Booking>();

        public static Booking FormatAndValidateBooking(Booking bookingData)
        {
            // 1. Trim and normalize strings
            bookingData.StudentfName = (bookingData.StudentfName ?? "").Trim();
            bookingData.StudentlName = (bookingData.StudentlName ?? "").Trim();
            bookingData.ContactNumber = (bookingData.ContactNumber ?? "").Trim();

            // 2. Capitalize student names
            if (!string.IsNullOrEmpty(bookingData.StudentfName))
                bookingData.StudentfName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(bookingData.StudentfName.ToLower());
            if (!string.IsNullOrEmpty(bookingData.StudentlName))
                bookingData.StudentlName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(bookingData.StudentlName.ToLower());

            // 3. Validate student number (must be 6 digits)
            string studentNumberStr = bookingData.studentNumber.ToString();
            if (studentNumberStr.Length != 6)
            {
                throw new ArgumentException("Invalid student number. Must be 6 digits.");
            }

            // 4. Validate contact number (at least 10 digits)
            if (bookingData.ContactNumber.Length < 10)
            {
                throw new ArgumentException("Invalid phone number. Must be at least 10 digits.");
            }

            // 5. Training validation
            if (!bookingData.hasCompletedRequiredTraining)
            {
                throw new ArgumentException("Student has not completed the required training.");
            }

            return bookingData;
        }

        static void print(string input)
        {
            Console.Write(input);
        }

        static void print(int input)
        {
            Console.Write(input);
        }

        static void print(double input)
        {
            Console.Write(input);
        }

        public static void VerifyBooking(List<Booking> bookings)
        {
            if (bookings == null || bookings.Count == 0)
            {
                print("No bookings found in the system.\n\n");
                return;
            }
            print("\n--- ALL CAPTURED BOOKINGS ---\n");
            int count = 1;
            foreach (var book in bookings)
            {
                print($"#{count}: {book.StudentfName} {book.StudentlName} ({book.studentNumber}) - {book.equipType} for {book.bookingDuration}h\n");
                count++;
            }
            print("---------------------------\n\n");
        }

        public static List<Booking> CaptureBookingRequests(List<Booking> data)
        {
            try
            {
                print("\n--- CAPTURE NEW BOOKING ---\n");
                print("Enter First Name: ");
                string name = Console.ReadLine();
                print("Enter Last Name: ");
                string lname = Console.ReadLine();
                print("Enter Student Number (6 digits): ");
                if (!int.TryParse(Console.ReadLine(), out int studentNum)) throw new ArgumentException("Invalid student number format.");
                
                print("Enter Year of Study: ");
                if (!int.TryParse(Console.ReadLine(), out int YOS)) throw new ArgumentException("Invalid year of study format.");

                print("Capturing details...\n");
                Thread.Sleep(500); // 0.5 seconds for UX
                
                print("Booking Duration (hours): ");
                if (!int.TryParse(Console.ReadLine(), out int bookingDuration)) throw new ArgumentException("Invalid duration format.");

                print("Equipment Type: ");
                string equipType = Console.ReadLine();

                print("Completed Required Training (y/n)? ");
                string hasTrainingStr = Console.ReadLine().ToLower();
                bool hasCompletedRequiredTraining = !string.IsNullOrEmpty(hasTrainingStr) && hasTrainingStr[0] == 'y';

                print("Contact Number: ");
                string cNum = Console.ReadLine();

                Booking booking = new Booking(name, bookingDuration, studentNum, cNum, lname, YOS, equipType, hasCompletedRequiredTraining);
                
                // We validate here to ensure data is clean, but we don't reject yet.
                // Rejection happens in the Evaluate phase.
                try {
                    booking = FormatAndValidateBooking(booking);
                    print("Data validated successfully.\n");
                } catch (ArgumentException ex) {
                    print($"Note: Validation warning - {ex.Message}\n");
                }

                data.Add(booking);
                print("Booking added to queue.\n\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError capturing booking: {ex.Message}");
            }

            return data;
        }

        public static void EvaluateBookingElgibility(List<Booking> allBookings)
        {
            if (allBookings == null || allBookings.Count == 0)
            {
                print("No bookings to evaluate.\n\n");
                return;
            }

            ApprovedBookings.Clear();
            RejectedBookings.Clear();

            var activeCountPerStudent = allBookings.GroupBy(b => b.studentNumber)
                .ToDictionary(g => g.Key, g => g.Count());
            
            int approvedCount = 0;
            int conditionallyApprovedCount = 0;
            int rejectedCount = 0;

            print("\n=== EVALUATING BOOKING ELIGIBILITY ===\n\n");

            foreach (var booking in allBookings)
            {
                int studentBookings = activeCountPerStudent[booking.studentNumber];
                bool hasTraining = booking.hasCompletedRequiredTraining;
                bool durationOk = booking.bookingDuration <= 6;
                bool studentNumOk = booking.studentNumber.ToString().Length == 6;
                bool tooManyBookings = studentBookings > 3;

                string status = "";
                string reason = "";

                if (!hasTraining) reason = "Missing training";
                else if (!durationOk) reason = "Duration exceeds 6h limit";
                else if (!studentNumOk) reason = "Invalid student number";
                else if (tooManyBookings) reason = "Student has too many active bookings (>3)";

                if (reason != "")
                {
                    status = $"REJECTED ({reason})";
                    rejectedCount++;
                    RejectedBookings.Add(booking);
                }
                else if (booking.bookingDuration > 4)
                {
                    status = "CONDITIONALLY APPROVED (Management Review Required)";
                    conditionallyApprovedCount++;
                    ApprovedBookings.Add(booking);
                }
                else
                {
                    status = "FULLY APPROVED";
                    approvedCount++;
                    ApprovedBookings.Add(booking);
                }

                print($"Student: {booking.StudentfName} {booking.StudentlName} ({booking.studentNumber})\n");
                print($"Status : {status}\n\n");
            }

            print("=== EVALUATION SUMMARY ===\n");
            print($"Total evaluated : {allBookings.Count}\n");
            print($"Fully Approved  : {approvedCount}\n");
            print($"Conditional     : {conditionallyApprovedCount}\n");
            print($"Rejected        : {rejectedCount}\n\n");
        }

        public static void DisplayBookingStats()
        {
            print("\n=== CURRENT STATISTICS ===\n");
            print($"Approved bookings: {ApprovedBookings.Count}\n");
            print($"Rejected bookings: {RejectedBookings.Count}\n\n");

            if (ApprovedBookings.Count == 0)
            {
                print("No approved bookings to display.\n\n");
                return;
            }

            var sortedApproved = ApprovedBookings.OrderBy(b => b.YearOfStudy)
                .ThenBy(b => b.bookingDuration)
                .ToList();
            var sortedRejected = ApprovedBookings.OrderBy(b => b.YearOfStudy).ThenBy(b => b.bookingDuration).ToList();

            print("=== APPROVED BOOKINGS (By Priority) ===\n\n");
            int rank = 1;
            foreach (var booking in sortedApproved)
            {
                string note = booking.bookingDuration > 4 ? " [CONDITIONAL]" : "";
                print($"{rank}. {booking.StudentfName} {booking.StudentlName} (Year {booking.YearOfStudy}){note}\n");
                print($"   {booking.equipType} for {booking.bookingDuration}h\n");
                rank++;
            }
            print("\n");

           
        }

        enum MenuOptions
        {
            CaptureBookingRequests = 1,
            EvaluateBookingElgibility,
            DisplayBookingStats,
            Exit
        }

        public static void DisplayMenu()
        {
            print("\n" + new string('=', 40) + "\n");
            print("     TechnoLab Booking System\n");
            print(new string('=', 40) + "\n");
            foreach (var option in Enum.GetValues(typeof(MenuOptions)))
            {
                print($"{(int)option} - {option.ToString()}\n");
            }
            print(new string('=', 40) + "\n");
        }

        public static void AddPredefinedBookings(List<Booking> bookings)
        {
            List<Booking> predefined = new List<Booking>
            {
                new Booking("Alice", 3, 102345, "555-123-0124", "Thompson", 2, "Drone", true),
                new Booking("Brian", 1, 109876, "555-987-6880", "Walker", 4, "VR Headset", false),
                new Booking("Catherine", 4, 107654, "555-456-7612", "Nguyen", 1, "3D Printer", true),
                new Booking("David", 2, 103210, "555-234-5354", "Patel", 3, "Microcontroller", false),
                new Booking("Eva", 1, 108765, "555-678-9412", "Kim", 2, "Drone", true),
                new Booking("Frank", 3, 101234, "555-345-6890", "Lopez", 4, "VR Headset", false),
                new Booking("Grace", 2, 106789, "555-789-0112", "Singh", 1, "3D Printer", true),
                new Booking("Henry", 4, 104321, "555-567-8009", "Brown", 3, "Microcontroller", false),
                new Booking("Isabel", 1, 105432, "555-432-1001", "Martinez", 2, "Drone", true),
                new Booking("Jack", 3, 110987, "555-876-5342", "Wilson", 4, "VR Headset", false),
            };

            foreach (var b in predefined)
            {
                bookings.Add(b);
            }
            print("Predefined bookings added to the system queue.\n");
        }

        static void Main(string[] args)
        {
            List<Booking> bookings = new List<Booking>();
            
            // Pass the list to AddPredefinedBookings so they are captured
            AddPredefinedBookings(bookings);

            while (true)
            {
                DisplayMenu();
                print("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    print("Invalid input. Please enter a number.\n");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        bool more = true;
                        while (more)
                        {
                            bookings = CaptureBookingRequests(bookings);
                            print("Add another (y/n)? ");
                            string res = Console.ReadLine().ToLower();
                            if (string.IsNullOrEmpty(res) || res[0] == 'n') more = false;
                        }
                        break;
                    case 2:
                        EvaluateBookingElgibility(bookings);
                        break;
                    case 3:
                        DisplayBookingStats();
                        break;
                    case 4:
                        print("\nGoodbye!\n");
                        return;
                    case 5:
                        VerifyBooking(bookings);
                        break;
                    default:
                        print("Invalid option. Please choose an option from the list below\n");
                        break;
                }
            }
        }
    }
}
