using System;
using NUnit.Framework;

using Calendar.Models  ;

namespace Calendar.Tests
{
    public class Tests
    {
        private CalendarModel _calendar;

        [SetUp]
        public void Setup()
        {
            _calendar = new CalendarModel();
        }

        [Test]
        public void NewCalendarHasNoEventsAtArbitraryDate()
        {
            var emptyEventList = _calendar.GetEventsAtDateTime(DateTime.UnixEpoch);
            //Assert.Pass();
            Assert.AreEqual(emptyEventList.Count,0);
        }
    }
}