using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG261_TechnoLab
{
    public class Student
    {

        // Declare your variables
        private string _firstName;
        private string _lastName;
        private int _studentNumber;
        private int _yearOfStudy;
        private string _contactNumber;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("First Name cannot be empty.");
                }
                _firstName = value.Trim();
            }

        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Last Name cannot be empty.");
                }
                _lastName = value.Trim();
            }
        }

        public int StudentNumber
        {
            get { return _studentNumber; }
            set
            {
                if (value.ToString().Length != 6)
                {
                    throw new ArgumentException("Student number must be exactly 6 digits.");
                }
                _studentNumber = value;
            }
        }

        public int YearOfStudy
        {
            get { return _yearOfStudy; }
            set
            {
                if (value < 1 || value > 6)
                {
                    throw new ArgumentException("Year of study must be between 1 and 6");
                }
                _yearOfStudy = value;
            }
        }

        public string ContactNumber
        {
            get
            {
                return _contactNumber;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < 10)
                {
                    throw new ArgumentException("Contact number must be 10 characters.");
                }
                _contactNumber = value.Trim();
            }
        }

        // get the full name (Name + Surname)
        public string FullName => $"{FirstName} {LastName}";

        public Student(string firstName, string lastName, int studentNumber, int yearOfStudy, string contactNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            StudentNumber = studentNumber;
            YearOfStudy = yearOfStudy;
            ContactNumber = contactNumber;
        }
    }
}
