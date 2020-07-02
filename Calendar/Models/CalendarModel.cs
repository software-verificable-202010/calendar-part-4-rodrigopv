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
        #region Fields
        private List<CalendarEvent> events;
        #endregion

        #region Properties
        public DateTime CurrentTime
        {
            get; set;
        }
        #endregion

        #region Methods
        public CalendarModel()
        {
            events = new List<CalendarEvent>();
        }

        public List<CalendarEvent> GetEventsAtDateTime(DateTime date)
        {
            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();

            var result = from calendarEvent in events
                where (calendarEvent.EventDate.Day == date.Day
                       && calendarEvent.EventDate.Month == date.Month
                       && calendarEvent.EventDate.Year == date.Year)
                select calendarEvent;
            return result.ToList();

        }

        public void AddEvent(CalendarEvent calendarEvent) 
        {
            events.Add(calendarEvent);
        }

        public void RemoveEvent(CalendarEvent calendarEvent)
        {
            events.Remove(calendarEvent);
        }
        #endregion
    }
}
