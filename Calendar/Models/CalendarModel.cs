using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Documents;

namespace Calendar.Models
{
    [Serializable]
    public class CalendarModel
    {
        private List<CalendarEvent> _events;
        public CalendarModel()
        {
            _events = new List<CalendarEvent>();
        }

        public DateTime CurrentTime { get; set; }

        public List<CalendarEvent> GetEventsAtDateTime(DateTime date)
        {
            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();

            var result = from calendarEvent in _events
                where (calendarEvent.EventDate.Day == date.Day
                       && calendarEvent.EventDate.Month == date.Month
                       && calendarEvent.EventDate.Year == date.Year)
                select calendarEvent;
            return result.ToList();

        }

        public void AddEvent(CalendarEvent calendarEvent) 
        {
            _events.Add(calendarEvent);
        }
    }
}
