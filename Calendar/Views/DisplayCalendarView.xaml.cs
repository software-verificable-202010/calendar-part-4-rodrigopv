
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Calendar.Models;
using Calendar.ViewModels;

namespace Calendar.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class DisplayCalendarView
    {
        #region Fields
        private readonly DisplayCalendarViewModel displayCalendarViewModel;
        private readonly CultureInfo applicationCulture = CultureInfo.GetCultureInfo(Constants.ApplicationCulture);
        #endregion

        #region Methods
        public DisplayCalendarView(string username)
        {
            displayCalendarViewModel = new DisplayCalendarViewModel();
            displayCalendarViewModel.SetLoggedUser(username);
            InitializeComponent();
            this.DataContext = displayCalendarViewModel;
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

        private void Button_Click_1(object sender, RoutedEventArgs e) => this.displayCalendarViewModel.NextButtonClick();

        private void Button_Click_2(object sender, RoutedEventArgs e) => this.displayCalendarViewModel.PreviousButtonClick();

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.displayCalendarViewModel.ToggleDisplayMode();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle selectedRectangle = (Rectangle)sender;
            CalendarEventViewModel selectedEvent = (CalendarEventViewModel)selectedRectangle.DataContext;

            string message = "";
            message += String.Format(applicationCulture, "Author: {0}\n", selectedEvent.Owner);
            message += String.Format(applicationCulture, "Starting hour: {0}\n", selectedEvent.GetStartTime());
            message += String.Format(applicationCulture, "Ending hour: {0}", selectedEvent.GetFinishTime());
            if (selectedEvent.Owner == displayCalendarViewModel.GetLoggedUser())
            {
                MessageBoxResult result = MessageBox.Show(message, Properties.Resources.editEventPrompt, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    NewEventView editDialog = new NewEventView(selectedEvent);
                    bool? editDialogResult = editDialog.ShowDialog();

                    if (editDialogResult == true)
                    {
                        if (editDialog.DeleteRequested)
                        {
                            displayCalendarViewModel.DeleteCalendarEvent(selectedEvent);
                            return;
                        }

                        List<string> ignoredUsers = DeleteScheduleConflicts(editDialog);
                        if (ignoredUsers.Count != 0)
                        {
                            string errorMessageString =
                                "Conflict detected on following user agendas: {0}. Invitation to conflict users has been ignored.";
                            string errorMessage = String.Format(
                                applicationCulture,
                                errorMessageString,
                                String.Join(",", ignoredUsers.ToArray()));
                            MessageBox.Show(errorMessage);
                        }

                        CalendarEvent calendarEvent = selectedEvent.CalendarEvent;
                        displayCalendarViewModel.UpdateCalendarEvent(calendarEvent, editDialog.TitleInput.Text, editDialog.PickedDate,editDialog.InvitedUsers,
                            editDialog.Description,editDialog.EndingHour, editDialog.EndingMinutes,editDialog.StartingHour,
                            editDialog.StartingMinutes);
                    }

                }
            }
            else
            {
                MessageBox.Show(message, Properties.Resources.userIsInvited, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No);
            }

        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Rectangle selectedRectangle)
            {
                CalendarEventViewModel selectedEvent = selectedRectangle.DataContext as CalendarEventViewModel;
                displayCalendarViewModel.SelectedEvent = selectedEvent;
            }


        }

        private void NewEvent_Button_Click(object sender, RoutedEventArgs e)
        {
            NewEventView newEventDialog = new NewEventView();
            bool? result = newEventDialog.ShowDialog();
            string currentUser = displayCalendarViewModel.GetLoggedUser();
            if (result == true)
            {
                List<string> ignoredUsers = DeleteScheduleConflicts(newEventDialog);
                if (ignoredUsers.Count != 0)
                {
                    string errorMessage = String.Format(
                        applicationCulture,
                        "Conflict detected on following user agendas: {0}. Invitation to conflict users has been ignored.",
                        String.Join(",", ignoredUsers.ToArray()));
                    MessageBox.Show(errorMessage);
                }
                CalendarEvent newEvent = new CalendarEvent(newEventDialog.TitleInput.Text, newEventDialog.PickedDate, newEventDialog.StartingHour, 
                    newEventDialog.StartingMinutes, newEventDialog.EndingHour, newEventDialog.EndingMinutes, currentUser,newEventDialog.Description,
                    newEventDialog.InvitedUsers);
                displayCalendarViewModel.AddEvent(newEvent);
            }
        }

        private List<String> DeleteScheduleConflicts(NewEventView newEventDialog)
        {
            int newEventStartTimeInMinutes = newEventDialog.StartingHour * Constants.HourInMinutes +
                             newEventDialog.StartingMinutes;
            int newEventEndTimeInMinutes = newEventDialog.EndingHour * Constants.HourInMinutes +
                                           newEventDialog.EndingMinutes;
            List<CalendarEvent> otherEvents = displayCalendarViewModel.GetEventsAtDateTime(newEventDialog.PickedDate);
            List<string> invitedUsers = new List<string>(newEventDialog.InvitedUsers.Split(","));
            List<string> ignoredUsers = new List<string>();
            foreach (string invitedUser in invitedUsers.ToArray())
            {
                foreach (CalendarEvent calendarEvent in otherEvents)
                {
                    if (newEventDialog.CalendarEventViewModel != null && newEventDialog.CalendarEventViewModel.CalendarEvent == calendarEvent)
                    {
                        // Make sure there is no conflict with the same event being edited.
                        continue;
                    }
                    List<string> relatedPeople = new List<string>();
                    var startTimeInMinutes = calendarEvent.GetStartingHour() * Constants.HourInMinutes +
                                             calendarEvent.GetStartingMinutes();
                    var endTimeInMinutes = calendarEvent.GetEndingHour() * Constants.HourInMinutes +
                                           calendarEvent.GetEndingMinutes();
                    relatedPeople.Add(calendarEvent.Owner);
                    relatedPeople.AddRange(calendarEvent.InvitedUsers.Split(','));

                    if (relatedPeople.Contains(invitedUser))
                    {
                        // New event starts when an event is already occurring.
                        if (newEventStartTimeInMinutes >= startTimeInMinutes && newEventStartTimeInMinutes < endTimeInMinutes)
                        {
                            ignoredUsers.Add(invitedUser);
                            invitedUsers.Remove(invitedUser);
                            break;
                        }
                        // New event starts before another event but ends within an existing one.
                        if (newEventEndTimeInMinutes > startTimeInMinutes && newEventEndTimeInMinutes <= endTimeInMinutes)
                        {
                            ignoredUsers.Add(invitedUser);
                            invitedUsers.Remove(invitedUser);
                            break;
                        }
                        // New event starts before another event and ends after another, but there's still an event between.
                        if (newEventStartTimeInMinutes < startTimeInMinutes && endTimeInMinutes < newEventEndTimeInMinutes)
                        {
                            ignoredUsers.Add(invitedUser);
                            invitedUsers.Remove(invitedUser);
                            break;
                        }
                    }
                }
            }

            newEventDialog.InvitedUsers = String.Join(",", invitedUsers.ToArray());
            return ignoredUsers;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button senderButton)
            {
                int selectedDay = Int32.Parse(senderButton.Content.ToString().Replace("*", "", System.StringComparison.InvariantCulture), applicationCulture );
                displayCalendarViewModel.CurrentDay = selectedDay;
            }
                
            displayCalendarViewModel.ToggleDisplayMode();
        }
        #endregion

    }
}
