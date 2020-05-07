using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Documents;

namespace Calendar.Models
{
    [Serializable]
    class CalendarModel
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
            CalendarEvent sampleEvent = new CalendarEvent("Reunion",DateTime.Today ,11,00,12, 00);
            CalendarEvent sampleEvent2 = new CalendarEvent("Reunion 2", DateTime.Today,  11, 00, 11, 30);

            var result = from calendarEvent in _events
                where (calendarEvent.EventDate.Day == date.Day
                       && calendarEvent.EventDate.Month == date.Month
                       && calendarEvent.EventDate.Day == date.Day)
                select calendarEvent;

            calendarEvents.Add(sampleEvent);
            calendarEvents.Add(sampleEvent2);
            return result.ToList();
            //return calendarEvents;
        }

        public void AddEvent(CalendarEvent calendarEvent) 
        {
            _events.Add(calendarEvent);
        }
    }
}
