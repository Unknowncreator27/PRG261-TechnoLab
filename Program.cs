
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRG261_TechnoLab
{

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
        public static List<Booking> ApprovedBookings = new List<Booking>();
        public static List<Booking> RejectedBookings = new List<Booking>();
        public static Booking FormatAndValidateBooking(Booking bookingData)
        {
            
            // This function is a helper to format and validate any information entered into the booking input

            //1. Trim and normalize strings
            bookingData.StudentfName = bookingData.StudentfName.Trim();
            bookingData.StudentlName = bookingData.StudentlName.Trim();
            
            bookingData.ContactNumber = bookingData.ContactNumber.Trim();
            // Capitalize student name
            bookingData.StudentfName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(bookingData.StudentfName.ToLower());
            bookingData.StudentlName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(bookingData.StudentlName.ToLower());
            // get student number length
            string studentNumberStr = bookingData.studentNumber.ToString();
            if (studentNumberStr.Length != 6)
            {
               
                throw new ArgumentException("Invalid student number.");
                
            }
            
           
            // 3. Validate contact number
            if(bookingData.ContactNumber.Length < 10) {
               
                throw new ArgumentException("Invalid phone number.");
                
            }

            if (!bookingData.hasCompletedRequiredTraining)
            {
           
                throw new ArgumentException("You do not have proper training for a booking.");
                
            }
            return bookingData;
        }
        static void print(string input)
        {
            
            Console.Write(input);

        }
        static void print(int input)
        {
            if (input.ToString().Contains("\n"))
            {
                Console.WriteLine(input);
            }
            Console.Write(input);

        }
        static void print(double input)
        {
            if (input.ToString().Contains("\n"))
            {
                Console.WriteLine(input);
            }
            Console.WriteLine(input);

        }
        // Method to show all bookings - REMOVE LATER
        public static void VerifyBooking(List<Booking> booking)
        {
            if(booking == null || booking.Count == 0)
            {
                print("No bookings found.\n\n");
                
                //Console.Clear();
                return;
            }
            print("\nExisting bookings: \n");
            int count = 1;
            foreach (var book in booking)
            {
                print($"Booking #{count}: \n");
                print($"Student name: {book.StudentfName} {book.StudentlName}\n");
                print($"Student Number: {book.studentNumber}\n");
                print($"Booking Duration: {book.bookingDuration} hours\n");
                print($"Contact Number: {book.ContactNumber}\n\n");
                count++;

            }
        }

        public static List<Booking> CaptureBookingRequests(List<Booking> data)
        {
            
            print("\nEnter your name: \n");
            string name = Console.ReadLine();
            print("What is your Surname? ");
            string lname = Console.ReadLine();
            print("Enter you student number: ");
            int studentNum = int.Parse(Console.ReadLine());
            print("What year of study are you in? \n");            
            int YOS = int.Parse(Console.ReadLine());
            print("Capturing your name, surname, student number and year of study\n\n");
            Thread.Sleep(5);
            print("Captured successfully.\n\n");
            print("How long would you like to book? ");
            // TODO: Add a booking table to display open slots
            int bookingDuration = int.Parse(Console.ReadLine());
            print("What equipment do you want to use? ");
            string equipType = Console.ReadLine();
            print("Do you have the required training? ");
            char hasTraining = Console.ReadLine()[0];
            bool hasCompletedRequiredTraining = hasTraining != 'n';
            print("If need be, how can we reach you (give your contact number): \n");
            string cNum = Console.ReadLine();

            try
            {

                Booking booking = new Booking(
                      name,           // studentName
                      bookingDuration, // bookingDuration
                      studentNum,     // studentNum
                      cNum,           // contactNumber
                      lname,          // lname
                      YOS,            // YearOfStudy
                      equipType,
                      hasCompletedRequiredTraining
                );    
                string bookingDateStr = Convert.ToString(bookingDuration);
                booking = FormatAndValidateBooking(booking);
                data.Add(booking);
                print("Booking created.\n\n");
                // Call confirmBooking()
                print($"Lastest booking:\nStudent: {booking.StudentfName} {booking.StudentlName}\n" +
                    $"Student Number: {booking.studentNumber}\n" +
                    $"Duration: {booking.bookingDuration} hours\n" +
                    $"Contact: {booking.ContactNumber}\n" +
                    $"Equipment to use: {booking.equipType}\n\n" +
                    $"Has training: {booking.hasCompletedRequiredTraining}\n\n");
                    
            } catch(ArgumentException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


            return data;


        }

        // 2. 
        public static void EvaluateBookingElgibility(List<Booking> allBookings)
        {
            if(allBookings == null || allBookings.Count == 0)
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

            print("=== EVALUATING BOOKING ELIGIBILITY ===\n\n");

            foreach (var booking in allBookings)
            {
                int activeBookings = 0;
                if (activeCountPerStudent.ContainsKey(booking.studentNumber))
                {
                    activeBookings = activeCountPerStudent[booking.studentNumber];
                }

                bool hasTraining = booking.hasCompletedRequiredTraining;
                bool durationOkForNormal = booking.bookingDuration <= 4;
                bool durationOkForConditional = booking.bookingDuration <= 6;
                bool tooManyBookings = activeBookings >= 3;
                bool durationTooLong = booking.bookingDuration > 6;

                string status = "";
                if (durationTooLong || tooManyBookings)
                {
                    status = "REJECTED";
                    rejectedCount++;
                    RejectedBookings.Add(booking);
                } else if(hasTraining && durationOkForConditional && !tooManyBookings)
                {
                    status = "Conditionally approved";
                    conditionallyApprovedCount++;
                    ApprovedBookings.Add(booking);
                } else
                {
                    status = "REJECTED (Missing training or other rules)";
                    rejectedCount++;
                    RejectedBookings.Add(booking);
                }
                print($"Student: {booking.StudentfName} {booking.StudentlName} ({booking.studentNumber})\n");
                print($"Duration: {booking.bookingDuration} hours | Training: {(hasTraining ? "Yes" : "No")}\n");
                print($"Active bookings for this student: {activeBookings}\n");
                print($"Status: {status}\n\n");
            }

            print("=== EVALUATION SUMMARY ===\n");
            print($"Total bookings evaluated : {allBookings.Count}\n");
            print($"Fully Approved           : {approvedCount}\n");
            print($"Conditionally Approved   : {conditionallyApprovedCount}\n");
            print($"Rejected                 : {rejectedCount}\n\n");

            if (conditionallyApprovedCount > 0)
            {
                print("Note: Conditionally approved bookings require management final approval.\n");
            }

        }

        //3. 
        public static void DisplayBookingStats(List<Booking> allBookings)
        {
            if (allBookings == null || allBookings.Count == 0)
            {
                print("No bookings to display stats for.\n\n");
                return;
            }

            int totalRequests = allBookings.Count;
            int approvedCount = ApprovedBookings.Count;
            int rejectedCount = RejectedBookings.Count;

            print("\n=== BOOKING STATISTICS ===\n\n");
            print($"Total booking requests   : {totalRequests}\n");
            print($"Approved bookings        : {approvedCount}\n");
            print($"Rejected bookings        : {rejectedCount}\n\n");
            print($"Pending management Review: {ApprovedBookings.Count(b => false)}\n\n"); // mark conditional bookings
            if (ApprovedBookings.Count == 0)
            {
                print("No approved bookings to show.\n\n");
                return;
            }

            var sortedApproved = ApprovedBookings.OrderBy(b => b.YearOfStudy)
                .ThenBy(b => GetActiveBookingCount(b, allBookings))
                .ThenBy(b => b.bookingDuration)
                .ToList();

            print("=== APPROVED BOOKINS (In priorty order) ===\n\n");
            int rank = 1;
            foreach (var booking in sortedApproved)
            {
                int activeCount = GetActiveBookingCount(booking, allBookings);
                string priorityNote = "";
                if(booking.bookingDuration > 5)
                {
                    priorityNote = " (Conditional - Management Review Required)";
                    
                }

                print($"Priority #{rank} :\n");
                print($"Student     : {booking.StudentfName} {booking.StudentlName}\n");
                print($"Student No  : {booking.studentNumber}\n");
                print($"Year of Study : {booking.YearOfStudy}\n");
                print($"Duration    : {booking.bookingDuration} hours{priorityNote}\n");
                print($"Active bookings : {activeCount}\n");
                print($"Equipment   : {booking.equipType}\n");
                print($"Training    : {(booking.hasCompletedRequiredTraining ? "Completed" : "Not Completed")}\n");
                print(new string('-', 50) + "\n");
                rank++;
            }
        }

        // Helper method to calculate active bookings for a student
        private static int GetActiveBookingCount(Booking booking, List<Booking> allBookings)
        {
            return allBookings.Count(b => b.studentNumber == booking.studentNumber);
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
            string borderTop = "";
            print("Hi there user. Welcome to our TechnoLab. How can we assist you today?\n");
            for (int i = 0; i < 10; i++)
            {
                i = borderTop.Length;
                i++;
                borderTop += "=";
                print(borderTop);
               

            }
            print("\n");
            foreach (var option in Enum.GetValues(typeof(MenuOptions)))
            {
                
                print($"{(int)option} - {option.ToString()}\n");
            }
            string borderBottom = "";

            for (int i = 0; i < 10; i++)
            {
                i = borderBottom.Length;
                i++;
                borderBottom += "=";
                print(borderBottom);


            }
            print("\n");

        }
        static void Main(string[] args)
        {
          List<Booking> bookings = new List<Booking>();
          bool isRunnng = true;
          bool continueBooking = true;

            while (isRunnng) {
                DisplayMenu();

                print("Enter an option: \n");
                int optionChosen = int.Parse(Console.ReadLine());

                
                switch (optionChosen)
                {
                    case 1:
                        
                        print($"You chose {Enum.GetNames(typeof(MenuOptions)).GetValue(optionChosen - 1)}\n\n");
                        Thread.Sleep(5);
                        while (continueBooking)
                        {
                            bookings = CaptureBookingRequests(bookings);
                            print("Would you like to add another booking (y/n)? ");
                            char userAns = Char.Parse(Console.ReadLine());

                            if(userAns == 'n')
                            {
                                continueBooking = false;
                                print("No more bookings will be added");
                            }
                        }
                        isRunnng = true;
                        break;
                        
                    case 2:
                        
                        print($"You chose {Enum.GetNames(typeof(MenuOptions)).GetValue(optionChosen - 1)}\n\n");
                        EvaluateBookingElgibility(bookings);
                        isRunnng = true;
                        break;
                    case 3:
                        print($"You chose {Enum.GetNames(typeof(MenuOptions)).GetValue(optionChosen - 1)}\n\n");
                        DisplayBookingStats(bookings);
                        isRunnng = true;
                        break;
                    case 4:
                        print("\n Call again soon.\n\n");
                        Environment.Exit(0);
                        break;
                    case 5:
                        print("\n=== SECRET METHOD CALLED ===\n\n");
                        VerifyBooking(bookings);
                        print("=== END OF SECRET METHOD ===\n\n");

                        break;
                    default:
                        print("Invalid option, choose from the list below:");
                        break;
                }

            }
        

        }
    }
}