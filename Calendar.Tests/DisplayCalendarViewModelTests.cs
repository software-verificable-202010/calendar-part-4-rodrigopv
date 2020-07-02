using System;
using NUnit.Framework;
using Calendar.Models;
using Calendar.ViewModels;


namespace Calendar.Tests
{
    static class DisplayCalendarViewModelTests
    {
        [Test]
        public static void HasValidConstructor()
        {

            DisplayCalendarViewModel viewModel = new DisplayCalendarViewModel();
            Assert.NotNull(viewModel);
        }
    }
}
