using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class TaskComment
    {
        string text;
        User author;
        DateTime writeTime;

        public TaskComment() { }
        public TaskComment(string text, User author, DateTime writeTime)
        {
            this.text = text;
            this.author = author;
            this.writeTime = writeTime;
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
        public User Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
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
    }
}
