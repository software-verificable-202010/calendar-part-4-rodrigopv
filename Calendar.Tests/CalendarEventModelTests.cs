using System;
using NUnit.Framework;

using Calendar.Models  ;

namespace Calendar.Tests
{
    public class CalendarEventModelTests
    {
        [Test]
        public void CalendarEventGettersReturnRightData()
        {
            int startingHour = 0;
            int endingHour = 1;
            int startingMinutes = 0;
            int endingMinutes = 30;
            int eventDuration = 90;

            CalendarEvent calendarEvent = new CalendarEvent("Test", DateTime.Now, startingHour, startingMinutes, endingHour, endingMinutes, "rodrigo", "description", "");

            Assert.AreEqual(startingHour, calendarEvent.GetStartingHour());
            Assert.AreEqual(endingHour, calendarEvent.GetEndingHour());
            Assert.AreEqual(startingMinutes, calendarEvent.GetStartingMinutes());
            Assert.AreEqual(endingMinutes, calendarEvent.GetEndingMinutes());
            Assert.AreEqual(eventDuration, calendarEvent.GetDurationInMinutes());
        }

        [Test]
        public void CalendarEventSettersMethodsWorkFine()
        {
            int startingHour = 0;
            int endingHour = 1;
            int startingMinutes = 0;
            int endingMinutes = 30;

            string title = "Test";
            string event_description = "description";
            string owner = "rodrigo";
            string invited_users = "foo";

            int newStartingHour = 1;
            int newEndingHour = 2;
            int newEndingMinutes = 45;
            int newStartingMinutes = 10;
            int newEventDuration = 95;

            CalendarEvent calendarEvent = new CalendarEvent(title, DateTime.Now, startingHour, startingMinutes, endingHour, endingMinutes, owner, event_description, invited_users);
            calendarEvent.SetEndingHour(newEndingHour);
            calendarEvent.SetEndingMinutes(newEndingMinutes);
            calendarEvent.SetStartingHour(newStartingHour);
            calendarEvent.SetStartingMinutes(newStartingMinutes);

            Assert.AreEqual(newStartingHour, calendarEvent.GetStartingHour());
            Assert.AreEqual(newEndingHour, calendarEvent.GetEndingHour());
            Assert.AreEqual(newStartingMinutes, calendarEvent.GetStartingMinutes());
            Assert.AreEqual(newEndingMinutes, calendarEvent.GetEndingMinutes());
            Assert.AreEqual(newEventDuration, calendarEvent.GetDurationInMinutes());
        }

        [Test]
        public void CalendarEventPropertiesWorkFine()
        {
            int startingHour = 0;
            int endingHour = 1;
            int startingMinutes = 0;
            int endingMinutes = 30;

            DateTime date = DateTime.Now;
            string title = "Test";
            string eventDescription = "description";
            string owner = "rodrigo";
            string invitedUsers = "foo";

            CalendarEvent calendarEvent = new CalendarEvent(title, DateTime.Now, startingHour, startingMinutes, endingHour, endingMinutes, owner, eventDescription, invitedUsers);

            string newTitle = "New Title";
            string newEventDescription = "New description";
            string newOwner = "foo";
            string newInvitedUsers = "rodrigo";
            DateTime newDate = DateTime.Today;

            Assert.AreEqual(calendarEvent.Description, eventDescription);
            Assert.AreEqual(calendarEvent.EventDate, date);
            Assert.AreEqual(calendarEvent.InvitedUsers, invitedUsers);
            Assert.AreEqual(calendarEvent.Owner, owner);
            Assert.AreEqual(calendarEvent.Title, title);

            // Test setter properties.
            calendarEvent.Title = newTitle;
            calendarEvent.Description = newEventDescription;
            calendarEvent.Owner = newOwner;
            calendarEvent.InvitedUsers = newInvitedUsers;
            calendarEvent.EventDate = newDate;

            Assert.AreEqual(calendarEvent.Description, newEventDescription);
            Assert.AreEqual(calendarEvent.EventDate, newDate);
            Assert.AreEqual(calendarEvent.InvitedUsers, newInvitedUsers);
            Assert.AreEqual(calendarEvent.Owner, newOwner);
            Assert.AreEqual(calendarEvent.Title, newTitle);



        }
    }
}