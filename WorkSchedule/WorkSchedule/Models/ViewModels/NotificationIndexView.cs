using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.ViewModels
{
    public class NotificationIndexView
    {
        public List<Notification> Notifications { get; set; }
        public int Filter { get; set; }
        public bool Inbox { get; set; }
    }
}
