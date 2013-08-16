using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media.Effects;

namespace ReminderCentre.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    /// 
    public partial class MainView : Window
    {
        public MainView()
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
