using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using ReminderCentre.Model;
using System.Windows.Input;
using System;

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
    class DetailsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DetailsViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            DeleteTaskCommand = new RelayCommand(() => SendSelectedTaskToDelete());
            DeleteSubtaskCommand = new RelayCommand(() => DeleteSubtask());
            AddSubtaskCommand = new RelayCommand(() => AddSubtask());
            FinishSubtaskCommand = new RelayCommand(() => FinishSubtask());
            AddSubtask_EnterKeyCommand = new RelayCommand<KeyEventArgs>((e) => { 
                if (e.Key == Key.Enter)
                    AddSubtask(); 
            });
            Messenger.Default.Register<Task>(this, "DataSending_from_TaskVM_to_DetailsVM_SelectedTask", false, (task) => 
            {
                if (task != null)
                {
                    IsHitTestVisible = true;
                    Opacity = 1.0;
                }
                else
                {
                    IsHitTestVisible = false;
                    Opacity = 0.6;
                }
                SelectedTask = task; 
            });
        }

        private double _Opacity = 0.6;

        public double Opacity
        {
            get { return _Opacity; }
            set 
            { 
                _Opacity = value;
                RaisePropertyChanged("Opacity");
            }
        }

        private bool _IsHitTestVisible = false;

        public bool IsHitTestVisible
        {
            get 
            { 
                return _IsHitTestVisible; 
            }
            set 
            {
                if (_IsHitTestVisible != value)
                {
                    _IsHitTestVisible = value;
                    RaisePropertyChanged("IsHitTestVisible");
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

        private Subtask _SelectedSubtask;

        public Subtask SelectedSubtask
        {
            get { return _SelectedSubtask; }
            set 
            {
                if (_SelectedSubtask != value )
                {
                    _SelectedSubtask = value;
                    RaisePropertyChanged("SelectedSubtask");
                }
            }
        }

        public RelayCommand DeleteSubtaskCommand { get; private set; }

        public void DeleteSubtask()
        {
            Subtask SubtaskToBeDeleted = SelectedSubtask;
            SelectedTask.SubtaskList.Remove(SubtaskToBeDeleted);
        }

        public void SendSelectedTaskToDelete()
        {
            if (SelectedTask != null)
                Messenger.Default.Send<MessengerToken>(MessengerToken.Delete_this_task, "RequestSending_from_DetailsVM_to_TaskVM_SelectedTask");
        }

        public RelayCommand DeleteTaskCommand { get; private set; }

        private Task _SelectedTask;

        public Task SelectedTask
        {
            get { return _SelectedTask; }
            set 
            {
                if (_SelectedTask != value)
                {
                    _SelectedTask = value;
                    RaisePropertyChanged("SelectedTask");
                }
            }
        }

        public RelayCommand<KeyEventArgs> AddSubtask_EnterKeyCommand { get; private set; }
        

        private string _NewSubtaskName = "";

        public string NewSubtaskName
        {
            get { return _NewSubtaskName; }
            set
            {
                if (_NewSubtaskName != value)
                {
                    _NewSubtaskName = value;
                    RaisePropertyChanged("NewSubtaskName");
                }
            }
        }
        

        public RelayCommand AddSubtaskCommand { get; private set; }

        private void AddSubtask()
        {
            if (SelectedTask != null)
            {
                SelectedTask.SubtaskList.Add(
                    new Subtask()
                    {
                        SubtaskName = string.IsNullOrEmpty(NewSubtaskName)? "Unnamed": NewSubtaskName,
                        IsFinished = false
                    });
                NewSubtaskName = string.Empty;
            }
        }

        public RelayCommand FinishSubtaskCommand { get; private set; }

        private void FinishSubtask()
        {
            for (int i = 0; SelectedTask != null && SelectedTask.SubtaskList != null && i < SelectedTask.SubtaskList.Count; i++)
            {
                if (SelectedTask.SubtaskList[i].IsFinished == true)
                {
                    //animation request here...
                    SelectedTask.SubtaskList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}