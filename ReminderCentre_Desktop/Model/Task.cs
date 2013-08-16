using System;
using System.Collections.ObjectModel;

namespace ReminderCentre.Model
{
    public class Task
    {
        public string TaskName { get; set; }
        public Boolean IsFinished { get; set; }
        public string TaskNote { get; set; }
        public ObservableCollection<Subtask> SubtaskList { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? RemindTime { get; set; }
        public string RemindTimeStr { get; set; }
        public override string ToString()
        {
            return TaskName;
        }
    }
}
