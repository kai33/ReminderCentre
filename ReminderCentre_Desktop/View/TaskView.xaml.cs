using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;
using ReminderCentre.ViewModel;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls.Primitives;

namespace ReminderCentre.View
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Grid
    {
        public TaskView()
        {
            InitializeComponent();
            Messenger.Default.Register<MessengerToken>(this, "RequestSending_from_TaskVM_to_TaskView_EditCategory", false,
                (msg) => Change_TabControl_DataTemplate(msg));
        }

        private void Change_TabControl_DataTemplate(MessengerToken msg)
        {
            if (msg != MessengerToken.Category_EditMode && msg != MessengerToken.Category_NormalMode)
                return;
            TabControl tb = this.FindName("TabControl") as TabControl;
            if (msg == MessengerToken.Category_EditMode)
            {
                Button bt = this.FindName("TabManagementToggle") as Button;
                bt.Content = "End edit";
                bt.Opacity = 0.5;
                bt.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4193D8"));
                bt.ToolTip = "End the edit";
                Style EditModeStyle = Resources["RC_TabControl.Style_EditMode_Original"] as Style;
                Style DisableModeStyle = Resources["RC_TabControl.Style_DisableMode_Original"] as Style;
                TabPanel tp = FindVisualChild<TabPanel>(TabControl as DependencyObject);
                foreach (TabItem item in tp.Children)
                {
                    DependencyObject DOitem = item as DependencyObject;
                    Label label = FindVisualChild<Label>(DOitem);
                    if (label == null)
                    {//it's a hack for adding new item, but not shown yet due to binding
                        item.Style = Resources["RC_TabControl.Style_NewItemMode_Original"] as Style;
                        item.IsSelected = true;
                        continue;
                    }
                    string labelStr = label.Content as string;
                    string str = labelStr;
                    if (str != "Inbox" && str != "Today" && str != "Log" && str != "Someday")
                        item.Style = EditModeStyle;
                    else
                        item.Style = DisableModeStyle;
                }
            }
            else
            {
                Button bt = this.FindName("TabManagementToggle") as Button;
                bt.Content = "Edit category";
                bt.Opacity = 1.0;
                bt.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA4A4A4"));
                bt.ToolTip = "Edit the category";
                Style NormalModeStyle = Resources["RC_TabControl.Style_NormalMode_Original"] as Style;
                TabPanel tp = FindVisualChild<TabPanel>(TabControl as DependencyObject);
                foreach (TabItem item in tp.Children)
                {
                    item.Style = NormalModeStyle;
                }
            }
        }

        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }
    }
}
