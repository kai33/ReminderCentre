using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReminderCentre.View
{
    /// <summary>
    /// Interaction logic for CategoryManagerView.xaml
    /// </summary>
    /// 
    public partial class CategoryManagerView : Window
    {
        public CategoryManagerView()
        {
            InitializeComponent();
        }

        private void TitleBar_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if( GetWindow(this).WindowState == WindowState.Normal )
                    this.WindowState = WindowState.Maximized;
                else if( GetWindow(this).WindowState == WindowState.Maximized )
                    this.WindowState = WindowState.Normal;
                e.Handled = true;
            }
        }

        private void Window_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
