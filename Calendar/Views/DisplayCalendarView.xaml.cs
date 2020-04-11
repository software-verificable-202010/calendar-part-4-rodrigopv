using System.Windows;
using System.Windows.Input;

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

        private void Button_Click_1(object sender, RoutedEventArgs e) => this._displayCalendarViewModel.SetNextMonth();

        private void Button_Click_2(object sender, RoutedEventArgs e) => this._displayCalendarViewModel.SetPreviousMonth();
    }
}
