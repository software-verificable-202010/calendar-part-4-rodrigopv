
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Calendar.Models;
using Calendar.ViewModels;

namespace Calendar.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DisplayCalendarView
    {
        private readonly DisplayCalendarViewModel _displayCalendarViewModel;

        public DisplayCalendarView()
        {
            _displayCalendarViewModel = new DisplayCalendarViewModel();
            InitializeComponent();
            this.DataContext = _displayCalendarViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void OnRectangleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Rectangle");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => this._displayCalendarViewModel.NextButtonClick();

        private void Button_Click_2(object sender, RoutedEventArgs e) => this._displayCalendarViewModel.PreviousButtonClick();

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this._displayCalendarViewModel.ToggleDisplayMode();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle selectedRectangle = (Rectangle)sender;
            CalendarEventViewModel selectedEvent = (CalendarEventViewModel)selectedRectangle.DataContext;
            string message = "Hora Inicio: " + selectedEvent.GetStartTime() + "\n";
            message += "Hora Fin: " + selectedEvent.GetFinishTime();

            MessageBox.Show(message, selectedEvent.Title);
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            Rectangle selectedRectangle = (Rectangle)sender;

            CalendarEventViewModel selectedEvent = (CalendarEventViewModel)selectedRectangle.DataContext;
            _displayCalendarViewModel.SelectedEvent = selectedEvent;

        }

        private void NewEvent_Button_Click(object sender, RoutedEventArgs e)
        {
            NewEventView newEventDialog = new NewEventView();

            if ((bool)newEventDialog.ShowDialog())
            {
                CalendarEvent newEvent = new CalendarEvent(newEventDialog.TitleInput.Text, newEventDialog.PickedDate, newEventDialog.StartingHour, 
                    newEventDialog.StartingMinutes, newEventDialog.EndingHour, newEventDialog.EndingMinutes);
                _displayCalendarViewModel.AddEvent(newEvent);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button) sender;
            _displayCalendarViewModel.CurrentDay = Int32.Parse(senderButton.Content.ToString());
            _displayCalendarViewModel.ToggleDisplayMode();
        }
    }
}
