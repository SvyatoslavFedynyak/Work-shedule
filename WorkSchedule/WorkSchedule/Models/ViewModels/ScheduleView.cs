using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.ViewModels
{
    public class ScheduleView
    {
        public Schedule Schedule { get; set; } = new Schedule();
        public List<TaskLine> TaskLines { get; set; } = new List<TaskLine>();
        public string WorkerTypes { get; set; }
        public string Workers { get; set; }
        public string SelectedWorkers { get; set; }
        public List<WorkerType> WorkerTypesEnumerable { get; set; } = new List<WorkerType>();
        public List<string> WorkersEnumerable { get; set; } = new List<string>();
        public List<string> SelectedWorkersEnumerable { get; set; } = new List<string>();
    }
}
