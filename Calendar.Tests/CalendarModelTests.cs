using System;
using NUnit.Framework;
using Calendar.Models;

namespace Calendar.Tests
{
    class CalendarModelTests
    {


        [Test]
        public void NewCalendarHasNoEventsAtArbitraryDate()
        {
            CalendarModel calendar = new CalendarModel();
            var emptyEventList = calendar.GetEventsAtDateTime(DateTime.UnixEpoch);
            //Assert.Pass();
            Assert.AreEqual(emptyEventList.Count, 0);
        }

        [Test]
        public void CalendarMethods()
        {
            CalendarModel calendar = new CalendarModel();
            var emptyEventList = calendar.GetEventsAtDateTime(DateTime.UnixEpoch);
            Assert.AreEqual(emptyEventList.Count, 0);

            int startingHour = 0;
            int endingHour = 1;
            int startingMinutes = 0;
            int endingMinutes = 30;
            DateTime eventDate = DateTime.Today;

            CalendarEvent calendarEvent = new CalendarEvent("Test", DateTime.Today, startingHour, startingMinutes, endingHour, endingMinutes, "rodrigo", "description", "");
            calendar.AddEvent(calendarEvent);

            int expectedEventsAtDateTime = 1;
            Assert.AreEqual(calendar.GetEventsAtDateTime(eventDate).Count, expectedEventsAtDateTime);

            int expectedEventsAfterEventRemoval = 0;
            calendar.RemoveEvent(calendarEvent);
            Assert.AreEqual(calendar.GetEventsAtDateTime(eventDate).Count, expectedEventsAfterEventRemoval);
        }

        [Test]
        public void CalendarProperties()
        {
            CalendarModel calendar = new CalendarModel();
            calendar.CurrentTime = DateTime.Today;

            Assert.AreEqual(calendar.CurrentTime, DateTime.Today);
        }

    }
}
