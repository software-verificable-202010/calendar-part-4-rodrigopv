using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Navigation;

namespace Calendar.Models
{
    [Serializable]
    public class CalendarEvent
    {
        private string title;
        private int startingHour;
        private int startingMinutes;

        private int endingHour;
        private int endingMinutes;
        private string owner;
        private string description;
        private string invitedUsers;

        private DateTime date;
        public CalendarEvent(string title, DateTime eventDate, int startingHour, int startingMinutes, int endingHour, int endingMinutes, string owner, string description, string invitedUsers)
        {
            this.title = title;
            this.date = eventDate;
            this.startingHour = startingHour;
            this.startingMinutes = startingMinutes;
            this.endingHour = endingHour;
            this.endingMinutes = endingMinutes;
            this.owner = owner;
            this.description = description;
            this.invitedUsers = invitedUsers;
        }

        public string Title {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public string Owner
        {
            get
            {
                return owner; 

            }
            set
            {
                owner = value;

            }
        }

        public int GetDurationInMinutes()
        {
            return (endingHour - startingHour) * Constants.HourInMinutes + endingMinutes - startingMinutes;
        }

        public DateTime EventDate
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
        public int GetStartingHour()
        {
            return startingHour;
        }

        public int GetStartingMinutes()
        {
            return startingMinutes;
        }

        public int GetEndingHour()
        {
            return endingHour;
        }

        public int GetEndingMinutes()
        {
            return endingMinutes;
        }

        public void SetStartingHour(int newStartingHour)
        {
            startingHour = newStartingHour;
        }
        public void SetEndingHour(int newEndingHour)
        {
            endingHour = newEndingHour;
        }
        public void SetStartingMinutes(int newStartingMinutes)
        {
            startingMinutes = newStartingMinutes;
        }
        public void SetEndingMinutes(int newEndingMinutes)
        {
            endingMinutes = newEndingMinutes;
        }

        public string InvitedUsers
        {
            get
            {
                return invitedUsers;
            }
            set
            {
                invitedUsers = value;
            }
        } 
    }
}
