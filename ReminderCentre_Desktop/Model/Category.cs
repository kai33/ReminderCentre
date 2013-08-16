using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ReminderCentre.Model
{
    public class Category : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Index { get; set; }
        private string _CategoryName;
        public string CategoryName {
            get
            {
                return _CategoryName;
            }
            set
            {
                if (_CategoryName != value)
                {
                    _CategoryName = value;
                    OnPropertyChanged("CategoryName");
                }
            }
        }
        public ObservableCollection<Task> TaskList { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
