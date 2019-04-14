using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class Task
    {
        string title;
        string desription;
        List<TaskComment> comments;
        List<TaskMaterial> materials;
        User owner;
        List<User> collaborators;
        DateTime start;
        DateTime end;
        int priority;

        public Task() { }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Desription
        {
            get { return desription; }
            set { desription = value; }
        }
        public List<TaskComment> Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public List<TaskMaterial> Materials
        {
            get { return materials; }
            set { materials = value; }
        }
        public User Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        public List<User> Collaborators
        {
            get { return collaborators; }
            set { collaborators = value; }
        }
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }
        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public void Delete()
        {
            foreach (User item in Collaborators)
            {
                item.Collaboarations.Remove(this);
            }
            foreach (TaskComment item in comments)
            {
                item.Delete();
            }
        }
    }
}