using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class Task
    {
        int id;
        string title;
        string desription;
        int ownerId;
        DateTime start;
        DateTime end;
        int priority;

        public Task() { }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
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
        public int OwnerID
        {
            get { return ownerId; }
            set { ownerId = value; }
        }
    }
}