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
using Color = System.Drawing.Color;

namespace Calendar.ViewModels
{
    class CalendarEventViewModel : BaseViewModel
    {
        private CalendarEvent _calendarEvent;
        private const int MaxEventWidth = 20;
        private const int MinEventWidth = 10;

        public CalendarEventViewModel(CalendarEvent calendarEvent)
        {
            _calendarEvent = calendarEvent;
            BackgroundColor = GetRandomColor();
            Random randomNumberGenerator = new Random();
            Width = randomNumberGenerator.Next(MinEventWidth, MaxEventWidth + 1);

        }

        public SolidColorBrush BackgroundColor { get; set; }
        public int Width { get; set; }

        public string GetStartTime()
        {
            return _calendarEvent.GetStartingHour().ToString("D2") + ":" + _calendarEvent.GetStartingMinutes().ToString("D2");
        }
        public string GetFinishTime()
        {
            return _calendarEvent.GetEndingHour().ToString("D2") + ":" + _calendarEvent.GetEndingMinutes().ToString("D2");
        }

        public string StartTime
        {
            get { return GetStartTime(); }
        }
        public string EndingTime
        {
            get { return GetFinishTime(); }
        }

        public string Title
        {
            get { return _calendarEvent.Title; }
        }

        public Thickness Margin
        {
            get
            {
                int topMargin = (int)Math.Ceiling(_calendarEvent.GetStartingHour() * Constants.HourSlotHeight) + (int)Math.Floor(_calendarEvent.GetStartingMinutes()*Constants.PerMinuteHeight);
                return new Thickness(0, topMargin, 0, 0);
            }
        }

        public int Height
        {
            get
            {
                var hourHeight = (int) Math.Floor(Constants.HourSlotHeight *
                                                   (_calendarEvent.GetEndingHour() - _calendarEvent.GetStartingHour()));
                var minutesHeight = (int) Math.Ceiling(Constants.PerMinuteHeight * _calendarEvent.GetEndingMinutes());
                return hourHeight + minutesHeight;
            }
        }

        private byte GetRandomByte()
        {
            Random randomNumberGenerator = new Random();
            return (byte)randomNumberGenerator.Next(byte.MaxValue + 1);
        }

        private SolidColorBrush GetRandomColor()
        {
            var color = System.Windows.Media.Color.FromArgb((byte)255, GetRandomByte(), GetRandomByte(), GetRandomByte());
            
            return new SolidColorBrush(color);
        }
    }


}
