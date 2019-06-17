using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.ViewModels
{
    public class DetailsView
    {
        public Schedule Schedule { get; set; }
        public List<TaskLine> TaskLines { get; set; }
        public List<WorkerType> WorkerTypes { get; set; }
    }
}
