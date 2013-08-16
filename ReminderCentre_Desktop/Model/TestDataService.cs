using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ReminderCentre.Model
{
    class TestDataService
    {
        public TestDataService()
        {
            ObservableCollection<Category> TestDB_Category = new ObservableCollection<Category>();
            ObservableCollection<Task> TestDB_Task = new ObservableCollection<Task>();
            ObservableCollection<Task> TestDB_Task2 = new ObservableCollection<Task>();
            ObservableCollection<Task> TestDB_Task3 = new ObservableCollection<Task>();
            ObservableCollection<Task> TestDB_Task4 = new ObservableCollection<Task>();
            ObservableCollection<Subtask> TestDB_Subtask = new ObservableCollection<Subtask>();

            for (int i = 0; i < 5; i++)
                TestDB_Subtask.Add(
                    new Subtask() { SubtaskName = "Subtask Name + Subtask Name + Subtask Name + " + i, IsFinished = false }
                );

            for (int i = 0; i < 15; i++)
                TestDB_Task.Add(
                    new Task() { TaskName = "Task Name + Task Name + Task Name + " + i, IsFinished = false, TaskNote = "Task Note\nTask Note", SubtaskList = TestDB_Subtask, DueDate = DateTime.Today, RemindTimeStr = "None" }
                    );
            for (int i = 0; i < 7; i++)
                TestDB_Task2.Add(
                    new Task() { TaskName = "Task Name + Task Name + Task Name + " + i, IsFinished = false, TaskNote = "Task Note\nTask Note", SubtaskList = TestDB_Subtask, DueDate = DateTime.Today, RemindTimeStr = "None" }
                    );
            for (int i = 0; i < 2; i++)
                TestDB_Task3.Add(
                    new Task() { TaskName = "Task Name + Task Name + Task Name + " + i, IsFinished = false, TaskNote = "Task Note\nTask Note", SubtaskList = TestDB_Subtask, DueDate = DateTime.Today, RemindTimeStr = "None" }
                    );
            for (int i = 0; i < 4; i++)
                TestDB_Task4.Add(
                    new Task() { TaskName = "Task Name + Task Name + Task Name + " + i, IsFinished = false, TaskNote = "Task Note\nTask Note", SubtaskList = TestDB_Subtask, DueDate = DateTime.Today, RemindTimeStr = "None" }
                    );
            TestDB_Category.Add(
                new Category() { CategoryName = "Inbox", TaskList = TestDB_Task, Index = Guid.NewGuid().ToString()}
                );
            TestDB_Category.Add(
                new Category() { CategoryName = "Today", TaskList = TestDB_Task2, Index = Guid.NewGuid().ToString() }
                );
            TestDB_Category.Add(
                new Category() { CategoryName = "Someday", TaskList = TestDB_Task3, Index = Guid.NewGuid().ToString() }
                );
            TestDB_Category.Add(
                new Category() { CategoryName = "ECE Club", TaskList = TestDB_Task4, Index = Guid.NewGuid().ToString() }
                );
            TestDB_Category.Add(
                new Category() { CategoryName = "Log", TaskList = new ObservableCollection<Task>(), Index = Guid.NewGuid().ToString() }
                );

            Output = TestDB_Category;
        }

        public ObservableCollection<Category> Output {get;set;}
    }

}
