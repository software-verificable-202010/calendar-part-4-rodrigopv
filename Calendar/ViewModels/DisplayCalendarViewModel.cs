using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using Calendar.Models;

namespace Calendar.ViewModels
{
    class DisplayCalendarViewModel : BaseViewModel
    {
        private readonly int DISPLAY_MONTH = 1;
        private readonly int DISPLAY_WEEK = 2;

        private int DECEMBER = 12;
        private int JANUARY = 1;

        private int _displayMode;
        private readonly CalendarModel _calendar;

        public DisplayCalendarViewModel()
        {
            if (File.Exists("calendar.dat"))
            {
                Stream fileStream = new FileStream("calendar.dat", FileMode.Open);
                BinaryFormatter deserializer = new BinaryFormatter();
                _calendar = (CalendarModel)deserializer.Deserialize(fileStream);
            }
            else
            {
                _calendar = new CalendarModel();
            }
            
            CurrentYear = DateTime.Today.Year;
            CurrentMonth = DateTime.Today.Month;
            CurrentDay = DateTime.Today.Day;
            
            _displayMode = DISPLAY_WEEK;
        }

        public String Title => String.Join(" ", GetCurrentMonthName(), CurrentYear.ToString());

        public Visibility MonthCalendarVisibleStatus
        {
            get
            {
                if (_displayMode == DISPLAY_MONTH) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public Visibility WeekCalendarVisibleStatus
        {
            get
            {
                if (_displayMode == DISPLAY_WEEK) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public bool DisplayMonthCalendar()
        {
            if(_displayMode == DISPLAY_MONTH)
                return true;

            return false;
        }
        public bool DisplayWeekCalendar()
        {
            if (_displayMode == DISPLAY_WEEK)
                return true;

            return false;
        }

        public void ToggleDisplayMode()
        {
            if (_displayMode == DISPLAY_MONTH)
            {
                _displayMode = DISPLAY_WEEK;
            } 
            else if (_displayMode == DISPLAY_WEEK)
            {
                _displayMode = DISPLAY_MONTH;
            }
            OnPropertyChanged(nameof(DisplayMonthCalendar));
            OnPropertyChanged(nameof(DisplayWeekCalendar));
            OnPropertyChanged(nameof(MonthCalendarVisibleStatus));
            OnPropertyChanged(nameof(WeekCalendarVisibleStatus));
            OnPropertyChanged(nameof(WeekDaySlots));

        } 



        public int CurrentYear
        {
            get => _calendar.CurrentTime.Year;
            set
            {
                _calendar.CurrentTime = new DateTime(value, _calendar.CurrentTime.Month, _calendar.CurrentTime.Day);
                BuildCalendarDaySlots();
                BuildEventSlots();
                BuildWeekDaysSlots();
                RefreshUIDayEventSlots();
                OnPropertyChanged(nameof(CurrentYear));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DaySlotList));
                OnPropertyChanged(nameof(WeekDaySlots));
            }
        }

        public int CurrentMonth
        {
            get => _calendar.CurrentTime.Month;
            set
            {
                _calendar.CurrentTime = new DateTime(CurrentYear, value, CurrentDay);
                BuildCalendarDaySlots();
                BuildEventSlots();
                BuildWeekDaysSlots();
                RefreshUIDayEventSlots();
                OnPropertyChanged(nameof(CurrentMonth));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DaySlotList));
                OnPropertyChanged(nameof(WeekDaySlots));
            }
        }

        public int CurrentDay
        {
            get => _calendar.CurrentTime.Day;
            set
            {
                _calendar.CurrentTime = new DateTime(CurrentYear, CurrentMonth, value);
                BuildEventSlots();
                BuildWeekDaysSlots();
                RefreshUIDayEventSlots();
                OnPropertyChanged(nameof(CurrentDay));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DaySlotList));
                OnPropertyChanged(nameof(WeekDaySlots));
            }
        }

        public DateTime CurrentDateTime
        {
            get => _calendar.CurrentTime;
            set
            {
                _calendar.CurrentTime = value;
                BuildCalendarDaySlots();
                BuildEventSlots();
                BuildWeekDaysSlots();
                OnPropertyChanged(nameof(CurrentDay));
                OnPropertyChanged(nameof(CurrentMonth));
                OnPropertyChanged(nameof(CurrentYear));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DaySlotList));
                OnPropertyChanged(nameof(WeekDaySlots));
                RefreshUIDayEventSlots();
            }
        }

        public List<string> DaySlotList { get; set; } = new List<string>();

        public void SetNextMonth()
        {
            if (CurrentMonth == DECEMBER)
            {
                CurrentYear += 1;
                CurrentMonth = JANUARY;
                return;
            }
            CurrentMonth += 1;
        }
        public void SetPreviousMonth()
        {
            if (CurrentMonth == JANUARY)
            {
                CurrentYear -= 1;
                CurrentMonth = DECEMBER;
            }
            else
            {
                CurrentMonth -= 1;
            }
        }

        public void SetNextWeek()
        {
            CurrentDateTime = CurrentDateTime.AddDays(7);
        }

        public void SetPreviousWeek()
        {
            CurrentDateTime = CurrentDateTime.AddDays(-7);
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
                // Offset for weeks beginning at Monday.
                // Sunday would be the last day, so fill 6 days.
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

        private void BuildWeekDayHourCalendarEventSlots(int day, int hour)
        {

        }

        private void BuildEventSlots()
        {
            MondayEventSlots = BuildEventSlotsByDay(DayOfWeek.Monday);
            TuesdayEventSlots = BuildEventSlotsByDay(DayOfWeek.Tuesday);
            WednesdayEventSlots = BuildEventSlotsByDay(DayOfWeek.Wednesday);
            ThursdayEventSlots = BuildEventSlotsByDay(DayOfWeek.Thursday);
            FridayEventSlots = BuildEventSlotsByDay(DayOfWeek.Friday);
            SaturdayEventSlots = BuildEventSlotsByDay(DayOfWeek.Saturday);
            SundayEventSlots = BuildEventSlotsByDay(DayOfWeek.Sunday);

        }

        private List<CalendarEventViewModel> BuildEventSlotsByDay(DayOfWeek day)
        {
            if (!Enum.IsDefined(typeof(DayOfWeek), day))
                throw new InvalidEnumArgumentException(nameof(day), (int) day, typeof(DayOfWeek));

            var dayEvents = new List<CalendarEventViewModel>();

            DateTime targetDateTime = GetWeekDay(day);

            foreach (var calendarEvent in _calendar.GetEventsAtDateTime(targetDateTime))
            {
                var dayEvent = new CalendarEventViewModel(calendarEvent);
                dayEvents.Add(dayEvent);
            }
            return dayEvents.OrderBy(o=>o.Width).Reverse().ToList();
        }


        private DateTime GetWeekFirstDay()
        {
            DayOfWeek currentDay = _calendar.CurrentTime.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Sunday;
            DateTime sunday = _calendar.CurrentTime.AddDays(-daysTillCurrentDay);

            return sunday.AddDays(1); // Monday
        }

        private DateTime GetWeekDay(DayOfWeek day)
        {
            DateTime monday = GetWeekFirstDay();
            switch (day)
            {
                case DayOfWeek.Monday:
                    return monday;
                case DayOfWeek.Tuesday:
                    return monday.AddDays(1);
                case DayOfWeek.Wednesday:
                    return monday.AddDays(2);
                case DayOfWeek.Thursday:
                    return monday.AddDays(3);
                case DayOfWeek.Friday:
                    return monday.AddDays(4);
                case DayOfWeek.Saturday:
                    return monday.AddDays(5);
                case DayOfWeek.Sunday:
                    return monday.AddDays(6);
            }
            
            return monday;
        }

        private void BuildWeekDaysSlots()
        {
            List<int> weekDays = new List<int>();
            DateTime monday = GetWeekFirstDay();
            DateTime tuesday = monday.AddDays(1);
            DateTime wednesday = tuesday.AddDays(1);
            DateTime thursday = wednesday.AddDays(1);
            DateTime friday = thursday.AddDays(1);
            DateTime saturday = friday.AddDays(1);
            DateTime sunday = saturday.AddDays(1);
            weekDays.Add(monday.Day);
            weekDays.Add(tuesday.Day);
            weekDays.Add(wednesday.Day);
            weekDays.Add(thursday.Day);
            weekDays.Add(friday.Day);
            weekDays.Add(saturday.Day);
            weekDays.Add(sunday.Day);

            WeekDaySlots = weekDays;
        }

        public void RefreshUIDayEventSlots()
        {
            OnPropertyChanged(nameof(MondayEventSlots));
            OnPropertyChanged(nameof(TuesdayEventSlots));
            OnPropertyChanged(nameof(WednesdayEventSlots));
            OnPropertyChanged(nameof(ThursdayEventSlots));
            OnPropertyChanged(nameof(FridayEventSlots));
            OnPropertyChanged(nameof(SaturdayEventSlots));
            OnPropertyChanged(nameof(SundayEventSlots));
        }

        public List<int> WeekDaySlots { get; set; } = new List<int>();
        public List<CalendarEventViewModel> MondayEventSlots { get; set; } = new List<CalendarEventViewModel>();
        public List<CalendarEventViewModel> TuesdayEventSlots { get; set; } = new List<CalendarEventViewModel>();
        public List<CalendarEventViewModel> WednesdayEventSlots { get; set; } = new List<CalendarEventViewModel>();
        public List<CalendarEventViewModel> ThursdayEventSlots { get; set; } = new List<CalendarEventViewModel>();
        public List<CalendarEventViewModel> FridayEventSlots { get; set; } = new List<CalendarEventViewModel>();
        public List<CalendarEventViewModel> SaturdayEventSlots { get; set; } = new List<CalendarEventViewModel>();
        public List<CalendarEventViewModel> SundayEventSlots { get; set; } = new List<CalendarEventViewModel>();

        private CalendarEventViewModel _selectedEvent;
        public CalendarEventViewModel SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }

        public void AddEvent(CalendarEvent newEvent)
        {
            _calendar.AddEvent(newEvent);
           
            BuildEventSlots();
            RefreshUIDayEventSlots();
            SaveCalendarToFile();
        }

        public void PreviousButtonClick()
        {
            if (_displayMode == DISPLAY_MONTH)
            {
                SetPreviousMonth();
            }
            else if (_displayMode == DISPLAY_WEEK)
            {
                SetPreviousWeek();
            }
        }

        public void NextButtonClick()
        {
            if (_displayMode == DISPLAY_MONTH)
            {
                SetNextMonth();
            }
            else if (_displayMode == DISPLAY_WEEK)
            {
                SetNextWeek();
            }
        }

        public void SaveCalendarToFile()
        {
            
            Stream fileToWrite = new FileStream("calendar.dat", FileMode.Create);
            BinaryFormatter binarySerializer = new BinaryFormatter();
            binarySerializer.Serialize(fileToWrite, _calendar);
            fileToWrite.Close();

        }
    }
}
