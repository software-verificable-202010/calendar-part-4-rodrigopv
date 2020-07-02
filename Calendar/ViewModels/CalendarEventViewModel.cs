using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Calendar.Models;
using Microsoft.VisualBasic;
using static Calendar.Constants;
using Color = System.Drawing.Color;

namespace Calendar.ViewModels
{
    public class CalendarEventViewModel : BaseViewModel
    {
        #region Constants
        private const int maxEventWidth = 20;
        private const int minEventWidth = 10;
        private readonly CultureInfo applicationCulture = CultureInfo.GetCultureInfo(Constants.ApplicationCulture);
        #endregion

        #region Fields
        private CalendarEvent calendarEvent;
        #endregion

        #region Properties
        public SolidColorBrush BackgroundColor
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }
        public string Description
        {
            get
            {
                return calendarEvent.Description;
            }
        }

        public string EventStartTime
        {
            get
            {
                return GetStartTime();
            }
        }
        public string EndingTime
        {
            get
            {
                return GetFinishTime();
            }
        }

        public string Title
        {
            get
            {
                return calendarEvent.Title;
            }
        }

        public Thickness Margin
        {
            get
            {
                int topMargin = (int)Math.Ceiling(calendarEvent.GetStartingHour() * Constants.HourSlotHeight) + (int)Math.Floor(calendarEvent.GetStartingMinutes() * Constants.PerMinuteHeight);
                return new Thickness(0, topMargin, 0, 0);
            }
        }

        public int Height
        {
            get
            {
                var hourHeight = (int)Math.Floor(Constants.HourSlotHeight *
                                                 (calendarEvent.GetEndingHour() - calendarEvent.GetStartingHour()));
                var minutesHeight = (int)Math.Ceiling(Constants.PerMinuteHeight * calendarEvent.GetEndingMinutes());
                return hourHeight + minutesHeight;
            }
        }

        public string Owner
        {
            get
            {
                return calendarEvent.Owner;

            }
            set
            {
                calendarEvent.Owner = value;

            }
        }

        public CalendarEvent CalendarEvent
        {
            get
            {
                return calendarEvent;

            }
        }
        #endregion

        #region Methods
        public CalendarEventViewModel(CalendarEvent calendarEvent)
        {
            this.calendarEvent = calendarEvent;
            BackgroundColor = GetRandomColor();
            Random randomNumberGenerator = new Random();
            Width = randomNumberGenerator.Next(minEventWidth, maxEventWidth + 1);

        }
        public string GetStartTime()
        {
            return string.Format(applicationCulture,"{0}:{1}",
                calendarEvent.GetStartingHour().ToString("D2", applicationCulture),
                calendarEvent.GetStartingMinutes().ToString("D2", applicationCulture));
        }
        public string GetFinishTime()
        {
            return string.Format(applicationCulture, "{0}:{1}",
                calendarEvent.GetEndingHour().ToString( "D2", applicationCulture),
                calendarEvent.GetEndingMinutes().ToString( "D2", applicationCulture));
        }
        private static byte GetRandomByte()
        {
            Random randomNumberGenerator = new Random();
            return (byte)randomNumberGenerator.Next(byte.MaxValue + 1);
        }

        private static SolidColorBrush GetRandomColor()
        {
            var color = System.Windows.Media.Color.FromArgb((byte)255, GetRandomByte(), GetRandomByte(), GetRandomByte());

            return new SolidColorBrush(color);
        }
        #endregion

    }


}
