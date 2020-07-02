using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calendar.ViewModels;

namespace Calendar.Views
{
    /// <summary>
    /// </summary>
    public partial class NewEventView : Window
    {
        private const int intMinValue = 0;
        private const int defaultEndingHourIndex = 4;
        private bool deleteRequested = false;
        private CalendarEventViewModel calendarEventViewModel;
        public NewEventView()
        {
            InitializeComponent();
            DateInput.DisplayDate = DateTime.Today;

            PopulateSelectBoxes();
            StartingHourInput.SelectedIndex = intMinValue;
            EndingHourInput.SelectedIndex = defaultEndingHourIndex;
            StartingMinutesInput.SelectedIndex = intMinValue;
            EndingMinutesInput.SelectedIndex = intMinValue;

            Title = "New Event Dialog";
        }

        public NewEventView(CalendarEventViewModel calendarEventViewModel)
        {
            InitializeComponent();
            DateInput.DisplayDate = DateTime.Today;
            PopulateSelectBoxes();
            if (calendarEventViewModel == null)
            {
                throw new NullReferenceException();
            }

            DateInput.SelectedDate = calendarEventViewModel.CalendarEvent.EventDate;
            DateInput.IsDropDownOpen = false;
            StartingHourInput.SelectedValue = calendarEventViewModel.CalendarEvent.GetStartingHour();
            StartingMinutesInput.SelectedValue = calendarEventViewModel.CalendarEvent.GetStartingMinutes();
            TitleInput.Text = calendarEventViewModel.Title;
            EndingHourInput.SelectedValue = calendarEventViewModel.CalendarEvent.GetEndingHour();
            EndingMinutesInput.SelectedValue = calendarEventViewModel.CalendarEvent.GetEndingMinutes();
            DescriptionInput.Text = calendarEventViewModel.CalendarEvent.Description;
            InvitedUsersInput.Text = calendarEventViewModel.CalendarEvent.InvitedUsers;
            Title = "Edit Event Dialog";
            DialogDeleteButton.Visibility = Visibility.Visible;
            this.calendarEventViewModel = calendarEventViewModel;


        }

        private void PopulateSelectBoxes()
        {

            List<int> availableHours = new List<int>();
            List<int> availableMinutes = new List<int>();

            for (int i = 0; i < Constants.HoursInDay; i++)
            {
                availableHours.Add(i);
            }
            for (int i = 0; i < Constants.HourInMinutes; i++)
            {
                availableMinutes.Add(i);
            }
            StartingHourInput.ItemsSource = availableHours;
            
            EndingHourInput.ItemsSource = availableHours;
            
            StartingMinutesInput.ItemsSource = availableMinutes;
            
            EndingMinutesInput.ItemsSource = availableMinutes;
            
        }

        public CalendarEventViewModel CalendarEventViewModel
        {
            get
            {
                return calendarEventViewModel;

            }
        }

        public bool DeleteRequested
        {
            get
            {
                return deleteRequested;

            }
        }

        public DateTime PickedDate
        {
            get
            {
                DateTime? pickedDate = DateInput.SelectedDate;
                if (pickedDate != null)
                {
                    return pickedDate.Value;
                }
                return DateTime.MinValue;
            }

        }

        public int StartingHour
        {
            get
            {
                if (StartingHourInput.SelectedValue is int startingHour)
                {
                    return startingHour;
                }

                return intMinValue;
            }
        }
        public int EndingHour
        {
            get
            {
                if (EndingHourInput.SelectedValue is int endingHour)
                {
                    return endingHour;
                }

                return intMinValue;
            }
        }
        public int StartingMinutes
        {
            get
            {
                if (StartingMinutesInput.SelectedValue is int startingMinutes)
                {
                    return startingMinutes;
                }

                return intMinValue;
            }
        }
        public int EndingMinutes
        {
            get
            {
                if (EndingMinutesInput.SelectedValue is int endingMinutes)
                {
                    return endingMinutes;
                }

                return intMinValue;
            }
        }

        public string Description
        {
            get
            {
                return DescriptionInput.Text;
            }
        }

        public string InvitedUsers
        {
            get
            {
                return InvitedUsersInput.Text;
            }
            set
            {
                InvitedUsersInput.Text = value;
            }

        }

        private void DialogSaveButton_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
        }

        private void DialogDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteRequested = true;
            this.DialogResult = true;
        }
    }
}
