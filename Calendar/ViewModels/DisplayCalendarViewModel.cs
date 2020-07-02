using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Calendar.Models;

namespace Calendar.ViewModels
{
    class DisplayCalendarViewModel : BaseViewModel
    {
        #region Constants
        private readonly int DISPLAY_MONTH = 1;
        private readonly int DISPLAY_WEEK = 2;

        private int DECEMBER = 12;
        private int JANUARY = 1;

        private CultureInfo applicationCulture = CultureInfo.GetCultureInfo(Constants.ApplicationCulture);
        #endregion

        #region Fields
        private string loggedUser = "";
        private int displayMode;
        private readonly CalendarModel calendar;
        public List<string> DaySlotList
        {
            get; set;
        } = new List<string>();
        private CalendarEventViewModel selectedEvent;
        #endregion

        public DisplayCalendarViewModel()
        {
            if (File.Exists("calendar_v2.dat"))
            {
                Stream fileStream = new FileStream("calendar_v2.dat", FileMode.Open);
                BinaryFormatter deserializer = new BinaryFormatter();
                calendar = (CalendarModel)deserializer.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                calendar = new CalendarModel();
            }
            
            CurrentYear = DateTime.Today.Year;
            CurrentMonth = DateTime.Today.Month;
            CurrentDay = DateTime.Today.Day;
            
            displayMode = DISPLAY_WEEK;
        }

        #region Properties
        public string Title
        {
            get
            {
                return String.Format(applicationCulture, "{0} {1}", GetCurrentMonthName(), CurrentYear.ToString(applicationCulture));
            }
        }

        public Visibility MonthCalendarVisibleStatus
        {
            get
            {
                if (displayMode == DISPLAY_MONTH)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public Visibility WeekCalendarVisibleStatus
        {
            get
            {
                if (displayMode == DISPLAY_WEEK)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public int CurrentYear
        {
            get
            {
                return calendar.CurrentTime.Year;
            }
            set
            {
                calendar.CurrentTime = new DateTime(value, calendar.CurrentTime.Month, calendar.CurrentTime.Day);
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
            get
            {
                return calendar.CurrentTime.Month;
            }
            set
            {
                calendar.CurrentTime = new DateTime(CurrentYear, value, CurrentDay);
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
            get
            {
                return calendar.CurrentTime.Day;
            }
            set
            {
                calendar.CurrentTime = new DateTime(CurrentYear, CurrentMonth, value);
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
            get
            {
                return calendar.CurrentTime;
            }
            set
            {
                calendar.CurrentTime = value;
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
        public CalendarEventViewModel SelectedEvent
        {
            get
            {
                return selectedEvent;
            }
            set
            {
                selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }

        #endregion

        #region Methods
        public void SetLoggedUser(string user)
        {
            
            loggedUser = user;
            BuildEventSlots();
            BuildCalendarDaySlots();
            RefreshUIDayEventSlots();
        }

        public string GetLoggedUser()
        {
            return loggedUser;
        }



        public bool DisplayMonthCalendar()
        {
            if(displayMode == DISPLAY_MONTH)
                return true;

            return false;
        }
        public bool DisplayWeekCalendar()
        {
            if (displayMode == DISPLAY_WEEK)
                return true;

            return false;
        }

        public void ToggleDisplayMode()
        {
            if (displayMode == DISPLAY_MONTH)
            {
                displayMode = DISPLAY_WEEK;
            } 
            else if (displayMode == DISPLAY_WEEK)
            {
                displayMode = DISPLAY_MONTH;
            }
            OnPropertyChanged(nameof(DisplayMonthCalendar));
            OnPropertyChanged(nameof(DisplayWeekCalendar));
            OnPropertyChanged(nameof(MonthCalendarVisibleStatus));
            OnPropertyChanged(nameof(WeekCalendarVisibleStatus));
            OnPropertyChanged(nameof(WeekDaySlots));

        }



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
            return calendar.CurrentTime.Day;
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

            return Convert.ToInt32(firstDayOfMonth.DayOfWeek, applicationCulture) - 1;

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
                DateTime day = new DateTime(CurrentYear, CurrentMonth, dayToAdd);
                bool markEventsAtDay = false;

                foreach(var calendarEvent in calendar.GetEventsAtDateTime(day))
                {
                    List<string> relatedPeople = new List<string>();
                    relatedPeople.Add(calendarEvent.Owner);
                    relatedPeople.AddRange(calendarEvent.InvitedUsers.Split(","));
                    if (relatedPeople.Contains(loggedUser))
                    {
                        markEventsAtDay = true;
                        break;
                    }
                }

                if(markEventsAtDay)
                {
                    daySlots.Add(String.Format(applicationCulture, "{0}*", dayToAdd.ToString(applicationCulture)));
                }
                else
                {
                    daySlots.Add(dayToAdd.ToString(applicationCulture));
                }
                
            }

            DaySlotList = daySlots;
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

            foreach (var calendarEvent in calendar.GetEventsAtDateTime(targetDateTime))
            {
                List<string> relatedPeople = new List<string>();
                relatedPeople.Add(calendarEvent.Owner);
                relatedPeople.AddRange(calendarEvent.InvitedUsers.Split(","));
                if(relatedPeople.Contains(loggedUser)) {
                    var dayEvent = new CalendarEventViewModel(calendarEvent);
                    dayEvents.Add(dayEvent);
                }

            }
            
            return dayEvents.OrderBy(o=>o.Width).Reverse().ToList();
        }


        private DateTime GetWeekFirstDay()
        {
            DayOfWeek currentDay = calendar.CurrentTime.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Sunday;
            DateTime sunday = calendar.CurrentTime.AddDays(-daysTillCurrentDay);

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

        public List<int> WeekDaySlots
        {
            get; set;
        } = new List<int>();

        public List<CalendarEventViewModel> MondayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();

        public List<CalendarEventViewModel> TuesdayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();

        public List<CalendarEventViewModel> WednesdayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();

        public List<CalendarEventViewModel> ThursdayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();

        public List<CalendarEventViewModel> FridayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();

        public List<CalendarEventViewModel> SaturdayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();

        public List<CalendarEventViewModel> SundayEventSlots
        {
            get; set;
        } = new List<CalendarEventViewModel>();




        public void AddEvent(CalendarEvent newEvent)
        {
            calendar.AddEvent(newEvent);
           
            BuildEventSlots();
            RefreshUIDayEventSlots();
            SaveCalendarToFile();
        }

        public void PreviousButtonClick()
        {
            if (displayMode == DISPLAY_MONTH)
            {
                SetPreviousMonth();
            }
            else if (displayMode == DISPLAY_WEEK)
            {
                SetPreviousWeek();
            }
        }

        public void NextButtonClick()
        {
            if (displayMode == DISPLAY_MONTH)
            {
                SetNextMonth();
            }
            else if (displayMode == DISPLAY_WEEK)
            {
                SetNextWeek();
            }
        }

        public void UpdateCalendarEvent(CalendarEvent calendarEvent, string newTitle, DateTime newDate, string newInvitedUsers, string newDescription, int newEndingHour, int newEndingMinutes, int newStartingHour, int newStartingMinutes)
        {
            calendarEvent.Title = newTitle;
            calendarEvent.EventDate = newDate;
            calendarEvent.InvitedUsers = newInvitedUsers;
            calendarEvent.Description = newDescription;
            calendarEvent.SetEndingHour(newEndingHour);
            calendarEvent.SetEndingMinutes(newEndingMinutes);
            calendarEvent.SetStartingHour(newStartingHour);
            calendarEvent.SetStartingMinutes(newStartingMinutes);

            BuildEventSlots();
            RefreshUIDayEventSlots();
            SaveCalendarToFile();
        }

        public void SaveCalendarToFile()
        {
            
            Stream fileToWrite = new FileStream("calendar_v2.dat", FileMode.Create);
            BinaryFormatter binarySerializer = new BinaryFormatter();
            binarySerializer.Serialize(fileToWrite, calendar);
            fileToWrite.Close();

        }

        public List<CalendarEvent> GetEventsAtDateTime(DateTime pickedDate)
        {
            return calendar.GetEventsAtDateTime(pickedDate);
        }

        public void DeleteCalendarEvent(CalendarEventViewModel calendarEventViewModel)
        {
            calendar.RemoveEvent(calendarEventViewModel.CalendarEvent);
            BuildEventSlots();
            RefreshUIDayEventSlots();
            SaveCalendarToFile();
        }
        #endregion
    }
}
