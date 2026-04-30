using PRG261_TechnoLab.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG261_TechnoLab
{
    internal class BookingValidator: IBookingValidator
    {
        public Booking FormatAndValidateBooking(Booking bookingData)
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
    }
}
