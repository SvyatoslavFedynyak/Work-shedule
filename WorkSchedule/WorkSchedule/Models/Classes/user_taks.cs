using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class user_taks
    {
        int id;
        int colaboratorId;
        int taskId;

        public user_taks() { }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int ColaboratorID
        {
            get { return colaboratorId; }
            set { colaboratorId = value; }
        }
        public int TaskID
        {
            get { return taskId; }
            set { taskId = value; }
        }
    }
}
