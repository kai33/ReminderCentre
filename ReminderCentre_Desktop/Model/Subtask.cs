using System;

namespace ReminderCentre.Model
{
    public class Subtask
    {
        public string SubtaskName { get; set; }
        public Boolean IsFinished { get; set; }
        public override string ToString()
        {
            return SubtaskName;
        }
    }
}
