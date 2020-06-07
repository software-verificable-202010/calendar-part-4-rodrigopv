using System;
using System.Collections.Generic;
using System.Text;

namespace Calendar
{
    static class Constants
    {
        // Height used per hour at week view. Used to calculate event dimensions.
        public const double HourSlotHeight = 25;
        public const int HourInMinutes = 60;
        public const double PerMinuteHeight = HourSlotHeight / HourInMinutes; // Height to add to event widget per each minute.
        public const int HoursInDay = 24;
    }
}
