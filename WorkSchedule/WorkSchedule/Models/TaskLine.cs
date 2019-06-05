using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models
{
    public class TaskLine
    {
        public int TaskLineID { get; set; }
        public int WorkerTypeID { get; set; }
        public string WorkerID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TaskID { get; set; }
        public DateTime Day { get; set; }
        public int TaskLineStatusID { get; set; }

        public virtual WorkerType WorkerType { get; set; }
    }
}
