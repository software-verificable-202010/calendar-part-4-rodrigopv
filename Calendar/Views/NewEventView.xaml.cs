using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calendar.Views
{
    /// <summary>
    /// </summary>
    public partial class NewEventView : Window
    {
        public NewEventView()
        {
            InitializeComponent();
            DateInput.DisplayDate = DateTime.Today;

            List<int> availableHours = new List<int>();
            List<int> availableMinutes = new List<int>();

            for (int i = 0; i < 24; i++)
            {
                availableHours.Add(i);
            }
            for (int i = 0; i < 60; i++)
            {
                availableMinutes.Add(i);
            }
            StartingHourInput.ItemsSource = availableHours;
            StartingHourInput.SelectedIndex = 0;

            EndingHourInput.ItemsSource = availableHours;
            EndingHourInput.SelectedIndex = 4;
            StartingMinutesInput.ItemsSource = availableMinutes;
            StartingMinutesInput.SelectedIndex = 0;
            EndingMinutesInput.ItemsSource = availableMinutes;
            EndingMinutesInput.SelectedIndex = 0;
        }

        public DateTime PickedDate
        {
            get { return (DateTime) DateInput.SelectedDate; }
        }

        public int StartingHour
        {
            get { return (int) StartingHourInput.SelectedValue; }
        }
        public int EndingHour
        {
            get { return (int)EndingHourInput.SelectedValue; }
        }
        public int StartingMinutes
        {
            get { return (int)StartingMinutesInput.SelectedValue; }
        }
        public int EndingMinutes
        {
            get { return (int)EndingMinutesInput.SelectedValue; }
        }

        private void DialogSaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
