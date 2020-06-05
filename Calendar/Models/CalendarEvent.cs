using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Navigation;

namespace Calendar.Models
{
    [Serializable]
    public class CalendarEvent
    {
        private string _title;
        private int _startingHour;
        private int _startingMinutes;
        private int _endingHour { get; }
        private int _endingMinutes;
        private DateTime _date;
        public CalendarEvent(string title, DateTime eventDate, int startingHour, int startingMinutes, int endingHour, int endingMinutes)
        {
            _title = title;
            _date = eventDate;
            _startingHour = startingHour;
            _startingMinutes = startingMinutes;
            _endingHour = endingHour;
            _endingMinutes = endingMinutes;
        }

        public string Title {
            get { return _title; }
            set { _title = value; }
        }

        public int GetDurationInMinutes()
        {
            return (_endingHour - _startingHour) * Constants.HourInMinutes + _endingMinutes - _startingMinutes;
        }

        public DateTime EventDate
        {
            get { return _date; }
        }
        public int GetStartingHour()
        {
            return _startingHour;
        }

        public int GetStartingMinutes()
        {
            return _startingMinutes;
        }

        public int GetEndingHour()
        {
            return _endingHour;
        }

        public int GetEndingMinutes()
        {
            return _endingMinutes;
        }
    }
}
