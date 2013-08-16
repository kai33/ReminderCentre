using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using ReminderCentre.Model;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using System.Xml.Serialization;

namespace ReminderCentre.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>

    class TaskViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the TaskViewModel class.
        /// </summary>
        public TaskViewModel()
        {
            if (IsInDesignMode)
            {
                TestDataService testData = new TestDataService();
                TaskData = testData.Output;
            }
            else
            {
                string FileName = @".\TaskData.xml";
                if(File.Exists(FileName)){
                    XmlSerializer reader = new XmlSerializer(typeof(ObservableCollection<Category>));
                    StreamReader file = new System.IO.StreamReader(FileName);
                    TaskData = reader.Deserialize(file) as ObservableCollection<Category>;
                }
                else
                {
                    TaskData = new ObservableCollection<Category>();
                    TaskData.Add(new Category()
                        { CategoryName = "Inbox", Index = Guid.NewGuid().ToString(), 
                            TaskList = new ObservableCollection<Task>()});
                    TaskData[0].TaskList.Add(new Task() { TaskName = "Welcome", TaskNote = "", IsFinished = false, SubtaskList = new ObservableCollection<Subtask>() }
                        );
                    TaskData.Add(new Category()
                        { CategoryName = "Today", Index = Guid.NewGuid().ToString(), TaskList = new ObservableCollection<Task>() });
                    TaskData.Add(new Category()
                        { CategoryName = "Someday", Index = Guid.NewGuid().ToString(), TaskList = new ObservableCollection<Task>() });
                    TaskData.Add(new Category()
                        { CategoryName = "Log", Index = Guid.NewGuid().ToString(), TaskList = new ObservableCollection<Task>() });
                }
            }

            Category_State = "Normal";

            AddTaskCommand = new RelayCommand(() => AddTask());
            FilterTaskCommand = new RelayCommand(() => FilterTask());
            ClearFilterCommand = new RelayCommand(() => { FilterStr = string.Empty; });
            AddTask_EnterKeyCommand = new RelayCommand<KeyEventArgs>((e) =>
                {
                    if (e.Key == Key.Enter)
                        AddTask();
                });

            EditCategory_DoubleClick_Command = new RelayCommand<MouseButtonEventArgs>((e) =>
                {
                    if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                    {
                        EditCategoryCommand.Execute(new object());
                        e.Handled = true;
                    }
                });

            EditCategory_Key_Command = new RelayCommand<KeyEventArgs>((e) =>
                {
                    if (e.Key == Key.Enter)
                        EditCategoryCommand.Execute(new object());
                });
            EditCategoryCommand = new RelayCommand(() =>
                {
                    if (Category_State == "Normal")
                    {
                        Messenger.Default.Send<MessengerToken>(MessengerToken.Category_EditMode, "RequestSending_from_TaskVM_to_TaskView_EditCategory");
                        Category_State = "Edit";
                    }
                    else
                    {
                        foreach (Category c in TaskData)
                        {
                            if (string.IsNullOrEmpty(c.CategoryName))
                                c.CategoryName = "Unnamed";
                        }
                        Messenger.Default.Send<MessengerToken>(MessengerToken.Category_NormalMode, "RequestSending_from_TaskVM_to_TaskView_EditCategory");
                        Category_State = "Normal";
                    }

                });
            DeleteCategoryCommand = new RelayCommand<string>((index) =>
                {
                    for (int i = 0; i < TaskData.Count; i++)
                    {
                        if (TaskData[i].Index == index)
                        {
                            TaskData.RemoveAt(i);
                            break;
                        }
                    }
                });
            AddCategoryCommand = new RelayCommand(() =>
                {
                    TaskData.Insert(TaskData.Count - 1, new Category() { CategoryName = "Unnamed", Index = Guid.NewGuid().ToString(), TaskList = new ObservableCollection<Task>() });
                    Messenger.Default.Send<MessengerToken>(MessengerToken.Category_EditMode, "RequestSending_from_TaskVM_to_TaskView_EditCategory");
                    Category_State = "Edit";
                });

            Messenger.Default.Register<MessengerToken>(this, "RequestSending_from_MainVM_to_TaskVM_SaveTaskData", false, (msg) =>
                {
                    if (msg == MessengerToken.Save)
                    {
                        try
                        {
                            XmlSerializer writer = new XmlSerializer(typeof(ObservableCollection<Category>));
                            StreamWriter file = new StreamWriter(@".\TaskData.xml");
                            writer.Serialize(file, TaskData);
                            file.Close();
                        }
                        catch (Exception e)
                        {
                            Exception ie = e.InnerException;
                        }
                    }
                });
            Messenger.Default.Register<MessengerToken>(this, "RequestSending_from_DetailsVM_to_TaskVM_SelectedTask", false, (msg) => { if (msg == MessengerToken.Delete_this_task) RemoveTask(); });
        }

        public RelayCommand<MouseButtonEventArgs> EditCategory_DoubleClick_Command { get; private set; }

        public RelayCommand<KeyEventArgs> EditCategory_Key_Command { get; private set; }

        public RelayCommand AddCategoryCommand { get; private set; }

        public RelayCommand<string> DeleteCategoryCommand { get; private set; }

        public RelayCommand EditCategoryCommand { get; private set; }

        public void RemoveTask()
        {
            Task TaskToBeDeleted = SelectedTask;
            if (SelectedCategory.TaskList.Count == 1)
                SelectedIndex = -1;
            else if (SelectedIndex == SelectedCategory.TaskList.Count - 1)
                SelectedIndex -= 1;
            else
                SelectedIndex += 1;
            SelectedCategory.TaskList.Remove(TaskToBeDeleted);
        }

        private string _FilterStr = "";

        public string FilterStr
        {
            get
            {
                return _FilterStr;
            }
            set
            {
                if (_FilterStr != value)
                {
                    _FilterStr = value.ToLower();
                    RaisePropertyChanged("FilterStr");
                }
            }
        }

        public RelayCommand ClearFilterCommand { get; private set; }

        public RelayCommand FilterTaskCommand { get; private set; }

        private void FilterTask()
        {
            if (SelectedCategory == null)
                return;
            CollectionView FilterList = (CollectionView)CollectionViewSource.GetDefaultView(SelectedCategory.TaskList);
            FilterList.Filter = new Predicate<object>((o) =>
            {
                Task t = o as Task;
                return t.TaskName.ToLower().Contains(FilterStr) || t.TaskNote.ToLower().Contains(FilterStr);
            });
        }

        private string _NewTaskName = "";

        public string NewTaskName
        {
            get
            {
                return _NewTaskName;
            }
            set
            {
                if (_NewTaskName != value)
                {
                    _NewTaskName = value;
                    RaisePropertyChanged("NewTaskName");
                }
            }
        }

        public RelayCommand AddTaskCommand { get; private set; }

        public RelayCommand<KeyEventArgs> AddTask_EnterKeyCommand { get; private set; }

        private void AddTask()
        {
            SelectedCategory.TaskList.Add(
                    new Task()
                    {
                        TaskName = string.IsNullOrEmpty(NewTaskName) ? "Unnamed" : NewTaskName,
                        IsFinished = false,
                        SubtaskList = new ObservableCollection<Subtask>(),
                        TaskNote = "",
                        DueDate = null,
                        RemindTimeStr = "None",
                    });
            NewTaskName = string.Empty;
            SelectedIndex = SelectedCategory.TaskList.Count - 1;
        }

        private Category _SelectedCategory;

        public Category SelectedCategory
        {
            get
            {
                return _SelectedCategory;
            }
            set
            {
                if (_SelectedCategory != value)
                {
                    _SelectedCategory = value;
                    FilterTask();
                }
            }
        }

        private Task _SelectedTask;

        public Task SelectedTask
        {
            get
            {
                return _SelectedTask;
            }
            set
            {
                if (_SelectedTask != value)
                {
                    _SelectedTask = value;
                    //make it into a function, and near to the registration region.
                    //--Messenger Registration Centre
                    //--Messenger Delivery Centre, etc.
                    Messenger.Default.Send<Task>(_SelectedTask, "DataSending_from_TaskVM_to_DetailsVM_SelectedTask");
                    RaisePropertyChanged("SelectedTask");
                }
            }
        }

        private int _SelectedIndex = -1;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    RaisePropertyChanged("SelectedIndex");
                }
            }
        }
        public ObservableCollection<Category> TaskData { get; set; }

        public string Category_State { get; set; }
    }
}