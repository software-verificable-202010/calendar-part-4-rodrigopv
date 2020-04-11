using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

using Calendar.Models;

namespace Calendar.ViewModels
{
    class DisplayCalendarViewModel : BaseViewModel
    {

        private readonly CalendarModel _calendar;

        public DisplayCalendarViewModel()
        {
            _calendar = new CalendarModel();
            CurrentYear = DateTime.Today.Year;
            CurrentMonth = DateTime.Today.Month;
            CurrentDay = DateTime.Today.Day;
        }

        public String Title => String.Join(" ", GetCurrentMonthName(), CurrentYear.ToString());


        public int CurrentYear
        {
            get => _calendar.CurrentTime.Year;
            set
            {
                _calendar.CurrentTime = new DateTime(value, _calendar.CurrentTime.Month, _calendar.CurrentTime.Day);
                BuildCalendarDaySlots();
                OnPropertyChanged(nameof(CurrentYear));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DaySlotList));
            }
        }

        public int CurrentMonth
        {
            get => _calendar.CurrentTime.Month;
            set
            {
                _calendar.CurrentTime = new DateTime(_calendar.CurrentTime.Year, value, _calendar.CurrentTime.Day);
                BuildCalendarDaySlots();
                OnPropertyChanged(nameof(CurrentMonth));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DaySlotList));
            }
        }

        public int CurrentDay
        {
            get => _calendar.CurrentTime.Day;
            set
            {
                _calendar.CurrentTime = new DateTime(_calendar.CurrentTime.Year, _calendar.CurrentTime.Month, value);
                OnPropertyChanged(nameof(CurrentDay));
                OnPropertyChanged(nameof(Title));
            }
        }

        public List<string> DaySlotList { get; set; } = new List<string>() { "hola"};

        public void SetNextMonth()
        {
            if (CurrentMonth == 12)
            {
                CurrentYear += 1;
                CurrentMonth = 1;
                return;
            }
            CurrentMonth += 1;
        }
        public void SetPreviousMonth()
        {
            if (CurrentMonth == 1)
            {
                CurrentYear -= 1;
                CurrentMonth = 12;
            }
            else
            {
                CurrentMonth -= 1;
            }
        }
        public int GetCurrentDay()
        {
            return _calendar.CurrentTime.Day;
        }

        private string GetCurrentMonthName()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentMonth);
        }

        private int GetDaysToFill()
        {
            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            if (firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
            {
                return 6;
            }

            return (int)firstDayOfMonth.DayOfWeek - 1;
        }

        private void BuildCalendarDaySlots()
        {
            var daySlots = new List<string>();
            for (var filledDays = 0; filledDays != GetDaysToFill(); filledDays++)
            {
                daySlots.Add(String.Empty);
            }

            for (var dayToAdd = 1; dayToAdd < DateTime.DaysInMonth(CurrentYear, CurrentMonth)+1; dayToAdd++)
            {
                daySlots.Add(dayToAdd.ToString());
            }

            DaySlotList = daySlots;
        }

    }
}
