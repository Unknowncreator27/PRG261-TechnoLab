using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


}
