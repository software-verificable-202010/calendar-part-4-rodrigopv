using Calendar.ViewModels;
using Calendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calendar.Tests
{
    class CalendarEventViewModelTests
    {
        [Test]
        public void HasValidConstructor()
        {
            int startingHour = 0;
            int endingHour = 1;
            int startingMinutes = 0;
            int endingMinutes = 30;
            DateTime eventDate = DateTime.Today;

            CalendarEvent calendarEvent = new CalendarEvent("Test", DateTime.Today, startingHour, startingMinutes, endingHour, endingMinutes, "rodrigo", "description", "");

            Assert.NotNull(new CalendarEventViewModel(calendarEvent));
        }

        [Test]
        public void TestProperties()
        {
            int startingHour = 0;
            int endingHour = 1;
            int startingMinutes = 0;
            int endingMinutes = 30;
            string owner = "rodrigo";
            string title = "Test";
            DateTime eventDate = DateTime.Today;

            CalendarEvent calendarEvent = new CalendarEvent(title, DateTime.Today, startingHour, startingMinutes, endingHour, endingMinutes, owner, "description", "");
            CalendarEventViewModel viewModel = new CalendarEventViewModel(calendarEvent);

            Assert.AreEqual(viewModel.Owner, owner);
            Assert.AreEqual(viewModel.Title, title);
        }
    }
}
