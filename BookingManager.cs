using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PRG261_TechnoLab
{

    internal class BookingManager
    {
        // These lists store the results of the evaluation process
        public static List<Booking> ApprovedBookings { get; private set; } = new List<Booking>();
        public static List<Booking> RejectedBookings { get; private set; } = new List<Booking>();
        private Write write = new Write();
        private BookingValidator validator = new BookingValidator();


        public BookingManager()
        {

        }

        public BookingManager(Write writer, BookingValidator Bookvalidator)
        {
            this.write = writer;
            this.validator = Bookvalidator;
        }
        public void VerifyBooking(List<Booking> bookings)
        {
            
            if (bookings == null || bookings.Count == 0)
            {
                write.print("No bookings found in the system.\n\n");
                return;
            }
            write.print("\n--- ALL CAPTURED BOOKINGS ---\n");
            int count = 1;
            foreach (var book in bookings)
            {
                write.print($"#{count}: {book.StudentfName} {book.StudentlName} ({book.studentNumber}) - {book.equipType} for {book.bookingDuration}h\n");
                count++;
            }
            write.print("---------------------------\n\n");
        }

        public List<Booking> CaptureBookingRequests(List<Booking> data)
        {
            try
            {
                write.print("\n--- CAPTURE NEW BOOKING ---\n");
                write.print("Enter First Name: ");
                string name = Console.ReadLine();
                write.print("Enter Last Name: ");
                string lname = Console.ReadLine();
                write.print("Enter Student Number (6 digits): ");
                if (!int.TryParse(Console.ReadLine(), out int studentNum)) throw new ArgumentException("Invalid student number format.");

                write.print("Enter Year of Study: ");
                if (!int.TryParse(Console.ReadLine(), out int YOS)) throw new ArgumentException("Invalid year of study format.");

                write.print("Capturing details...\n");
                Thread.Sleep(500); // 0.5 seconds for UX

                write.print("Booking Duration (hours): ");
                if (!int.TryParse(Console.ReadLine(), out int bookingDuration)) throw new ArgumentException("Invalid duration format.");

                write.print("Equipment Type: ");
                string equipType = Console.ReadLine();

                write.print("Completed Required Training (y/n)? ");
                string hasTrainingStr = Console.ReadLine().ToLower();
                bool hasCompletedRequiredTraining = !string.IsNullOrEmpty(hasTrainingStr) && hasTrainingStr[0] == 'y';

                write.print("Contact Number: ");
                string cNum = Console.ReadLine();

                Booking booking = new Booking(name, bookingDuration, studentNum, cNum, lname, YOS, equipType, hasCompletedRequiredTraining);

                // We validate here to ensure data is clean, but we don't reject yet.
                // Rejection happens in the Evaluate phase.
                try
                {
                    booking = validator.FormatAndValidateBooking(booking);
                    write.print("Data validated successfully.\n");
                }
                catch (ArgumentException ex)
                {
                    write.print($"Note: Validation warning - {ex.Message}\n");
                }

                data.Add(booking);
                write.print("Booking added to queue.\n\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError capturing booking: {ex.Message}");
            }

            return data;
        }

        public void EvaluateBookingElgibility(List<Booking> allBookings)
        {
            if (allBookings == null || allBookings.Count == 0)
            {
                write.print("No bookings to evaluate.\n\n");
                return;
            }

            ApprovedBookings.Clear();
            RejectedBookings.Clear();

            var activeCountPerStudent = allBookings.GroupBy(b => b.studentNumber)
                .ToDictionary(g => g.Key, g => g.Count());

            int approvedCount = 0;
            int conditionallyApprovedCount = 0;
            int rejectedCount = 0;

            write.print("\n=== EVALUATING BOOKING ELIGIBILITY ===\n\n");

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

                write.print($"Student: {booking.StudentfName} {booking.StudentlName} ({booking.studentNumber})\n");
                write.print($"Status : {status}\n\n");
            }

            write.print("=== EVALUATION SUMMARY ===\n");
            write.print($"Total evaluated : {allBookings.Count}\n");
            write.print($"Fully Approved  : {approvedCount}\n");
            write.print($"Conditional     : {conditionallyApprovedCount}\n");
            write.print($"Rejected        : {rejectedCount}\n\n");
        }

        public void DisplayBookingStats()
        {
            write.print("\n=== CURRENT STATISTICS ===\n");
            write.print($"Approved bookings: {ApprovedBookings.Count}\n");
            write.print($"Rejected bookings: {RejectedBookings.Count}\n\n");

            if (ApprovedBookings.Count == 0)
            {
                write.print("No approved bookings to display.\n\n");
                return;
            }

            var sortedApproved = ApprovedBookings.OrderBy(b => b.YearOfStudy)
                .ThenBy(b => b.bookingDuration)
                .ToList();
            var sortedRejected = ApprovedBookings.OrderBy(b => b.YearOfStudy).ThenBy(b => b.bookingDuration).ToList();

            write.print("=== APPROVED BOOKINGS (By Priority) ===\n\n");
            int rank = 1;
            foreach (var booking in sortedApproved)
            {
                string note = booking.bookingDuration > 4 ? " [CONDITIONAL]" : "";
                write.print($"{rank}. {booking.StudentfName} {booking.StudentlName} (Year {booking.YearOfStudy}){note}\n");
                write.print($"   {booking.equipType} for {booking.bookingDuration}h\n");
                rank++;
            }
            write.print("\n");


        }

        public void AddPredefinedBookings(List<Booking> bookings)
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
            write.print("Predefined bookings added to the system queue.\n");

        }


    }
}
