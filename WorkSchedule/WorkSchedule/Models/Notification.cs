using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string SenderID { get; set; }
        public string ToID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int TaskLineID { get; set; }

        public virtual TaskLine TaskLine { get; set; }
    }
}
