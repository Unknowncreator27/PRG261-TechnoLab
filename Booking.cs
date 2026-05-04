using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PRG261_TechnoLab
{
    public class Booking
    {
        // Private backing fields for encapsulation
        private int _bookingDuration;
        private string _equipType;

        // OOP: Student data lives in its own dedicated class
        public Student Student { get; set; }

        public int BookingDuration
        {
            get { return _bookingDuration; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Booking duration must be greater than 0.");
                _bookingDuration = value;
            }
        }

        public string EquipType
        {
            get { return _equipType; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Equipment type cannot be empty.");
                _equipType = value.Trim();
            }
        }

        public bool HasCompletedRequiredTraining { get; set; }

        // Backwards-compatible aliases so the rest of the code doesn't break
        public string StudentfName
        {
            get { return Student?.FirstName; }
            set { if (Student != null) Student.FirstName = value; }
        }
        public string StudentlName
        {
            get { return Student?.LastName; }
            set { if (Student != null) Student.LastName = value; }
        }
        public int studentNumber
        {
            get { return Student?.StudentNumber ?? 0; }
            set { if (Student != null) Student.StudentNumber = value; }
        }
        public int YearOfStudy
        {
            get { return Student?.YearOfStudy ?? 0; }
            set { if (Student != null) Student.YearOfStudy = value; }
        }
        public string ContactNumber
        {
            get { return Student?.ContactNumber; }
            set { if (Student != null) Student.ContactNumber = value; }
        }
        public int bookingDuration
        {
            get { return BookingDuration; }
            set { BookingDuration = value; }
        }
        public string equipType
        {
            get { return EquipType; }
            set { EquipType = value; }
        }
        public bool hasCompletedRequiredTraining
        {
            get { return HasCompletedRequiredTraining; }
            set { HasCompletedRequiredTraining = value; }
        }

        public Booking(string firstName, int bookingDuration, int studentNumber, string contactNumber,
                       string lastName, int yearOfStudy, string equipType, bool hasCompletedRequiredTraining)
        {
            Student = new Student(firstName, lastName, studentNumber, yearOfStudy, contactNumber);
            BookingDuration = bookingDuration;
            EquipType = equipType;
            HasCompletedRequiredTraining = hasCompletedRequiredTraining;
        }
    }

}
