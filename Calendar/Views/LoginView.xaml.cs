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
    /// Interaction logic for LoginView.xaml.
    /// </summary>
    public partial class LoginView : Window
    {
        #region Methods
        public LoginView()
        {
            InitializeComponent();
        }

        private void DoLogin(object sender, MouseButtonEventArgs e)
        {
            var calendarWindow = new DisplayCalendarView(InputUsername.Text);
            calendarWindow.Show();
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Gets the written letter as char as event sends a length-1 string.
            var writtenChar = e.Text[0];

            if (! char.IsLetter(writtenChar))
            {
                e.Handled = true;
            }
        }
        #endregion

    }
}
