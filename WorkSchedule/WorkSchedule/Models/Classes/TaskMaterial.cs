using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public enum MaterialType
    {LINK, FILE}

    public class TaskMaterial
    {
        int id;
        MaterialType type;
        string uri;
        int taskId;

        public TaskMaterial() { }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public MaterialType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public string Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }
        public int TaskID
        {
            get { return taskId; }
            set { taskId = value; }
        }
    }
}
