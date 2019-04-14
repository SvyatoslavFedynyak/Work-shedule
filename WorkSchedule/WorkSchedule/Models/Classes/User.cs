using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class User
    {
        protected string login;
        protected string password;
        protected List<Task> tasks;
        protected List<Task> collaboarations;

        public User() { }
        public User(string login, string password, List<Task> tasks, List<Task> collaboarations)
        {
            this.login = login;
            this.password = password;
            this.tasks = tasks;
            this.collaboarations = collaboarations;
        }

        public List<Task> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
            }
        }
        public List<Task> Collaboarations
        {
            get
            {
                return collaboarations;
            }
            set
            {
                collaboarations = value;
            }
        }
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public void CreateTask(string title, DateTime start, DateTime end)
        {
            Task newTask = new Task();
            newTask.Title = title;
            newTask.Start = start;
            newTask.End = end;
            newTask.Owner = this;
            tasks.Add(newTask);
        }
        public void RemoveTask(Task target)
        {
            target.Delete();
            tasks.Remove(target);
        }
        public void EditDesciption(string newDesc, Task target)
        {
            target.Desription = newDesc;
        }
        public void AddMaterial(Task target, MaterialType type, string uri)
        {
            target.Materials.Add(new TaskMaterial(type, uri));
        }
        public void RemoveMaterial(Task target, TaskMaterial material)
        {
            target.Materials.Remove(material);
        }
        public void EditTimeLim(DateTime newStart, DateTime newEnd, Task target)
        {
            target.Start = newStart;
            target.End = newEnd;
        }
        public void AddColaborator(Task target, User collaborator)
        {
            target.Collaborators.Add(collaborator);
            collaborator.collaboarations.Add(target);
        }
        public void Comment(Task target, string comment)
        {
            target.Comments.Add(new TaskComment(comment, this, DateTime.Now));
        }
        public void EditPriority(Task target, int newPrior)
        {
            target.Priority = newPrior;
        }
    }
}
