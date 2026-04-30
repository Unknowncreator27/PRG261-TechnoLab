using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG261_TechnoLab.interfaces
{
    public interface IBookingValidator
    {
         Booking FormatAndValidateBooking(Booking booking);

    }
}
