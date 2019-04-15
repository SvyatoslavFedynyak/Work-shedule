using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class TaskComment
    {
        int id;
        string text;
        int authorId;
        DateTime writeTime;
        int taskId;

        public TaskComment() { }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public DateTime WriteTime
        {
            get
            {
                return writeTime;
            }
            set
            {
                writeTime = value;
            }
        }
        public int TaskID
        {
            get { return taskId; }
            set { taskId = value; }
        }
        public int AuthorID
        {
            get { return authorId; }
            set { authorId = value; }
        }
    }
}
