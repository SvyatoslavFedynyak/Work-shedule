using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkSchedule.Data;
using WorkSchedule.Models;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Text;
using WorkSchedule.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkSchedule.Controllers
{
    public class TasksController : Controller
    {
        Schedule Schedule { get; set; }
        List<TaskLine> TaskLines { get; set; }

        private readonly ScheduleContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksController(ScheduleContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Schedule> Tasks = new List<Schedule>();
            Tasks = _context.Schedules.ToList() ?? new List<Schedule>();
            return View(Tasks);
        }

        [HttpGet]
        async public Task<IActionResult> Create()
        {
            ScheduleView t = new ScheduleView();
            var workers = await _userManager.GetUsersForClaimAsync(new Claim("Manager", User.Identity.Name));
            var Workers = workers.ToList() ?? new List<IdentityUser>();
            var WorkerTypes = _context.WorkerTypes.ToList() ?? new List<WorkerType>();
            var WorkersEmails = Workers.Select(x => x.Email).Distinct().ToList() ?? new List<string>();
            t.Workers = JsonConvert.SerializeObject(WorkersEmails, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            t.WorkerTypes = JsonConvert.SerializeObject(WorkerTypes, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            t.Schedule.From = DateTime.Now;
            t.Schedule.To = DateTime.Now;
            return View(t);
        }

        [HttpGet]
        async public Task<IActionResult> Edit(int id)
        {
            ScheduleView t = new ScheduleView();
            var Schedule = await _context.Schedules.SingleOrDefaultAsync(x => x.TaskID == id);
            var TaskLines = _context.TaskLines.Where(x => x.TaskID == id).ToList() ?? new List<TaskLine>();
            var workers = await _userManager.GetUsersForClaimAsync(new Claim("Manager", User.Identity.Name));
            var WorkerTypes = _context.WorkerTypes.ToList() ?? new List<WorkerType>();
            t.SelectedWorkersEnumerable = TaskLines.Select(x => x.WorkerID).ToList() ?? new List<string>();
            t.SelectedWorkers = JsonConvert.SerializeObject(t.SelectedWorkersEnumerable, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            t.WorkerTypes = JsonConvert.SerializeObject(WorkerTypes, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }).Replace("$", "");
            t.WorkerTypesEnumerable = WorkerTypes.ToList() ?? new List<WorkerType>();
            t.Schedule = Schedule;
            t.TaskLines = TaskLines;
            return View(t);
        }

        [HttpPost]
        async public Task<IActionResult> Edit(ScheduleView model)
        {
            var Schedule = model.Schedule;
            var TaskLines = model.TaskLines;
            Schedule.ModifiedBy = User.Identity.Name;
            Schedule.DateModified = DateTime.Now;
            _context.Schedules.Attach(Schedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var oldLines = _context.TaskLines.AsNoTracking().Where(x => x.TaskID == Schedule.TaskID).ToList() ?? new List<TaskLine>();
            for (int i = 0; i < TaskLines.Count; i++)
            {
                if (TaskLines[i].TaskLineID != 0)
                {
                    TaskLines[i].TaskID = Schedule.TaskID;
                    if (TaskLines[i].WorkerID == null)
                    {
                        TaskLines[i].WorkerID = "";
                    }


                    var oldLine = oldLines.SingleOrDefault(x => x.TaskLineID == TaskLines[i].TaskLineID);
                    if (oldLine.From != TaskLines[i].From || oldLine.To != TaskLines[i].To || oldLine.Day != TaskLines[i].Day || oldLine.WorkerID != TaskLines[i].WorkerID || oldLine.WorkerTypeID != TaskLines[i].WorkerTypeID)
                    {
                        TaskLines[i].TaskLineStatusID = 1;
                        _context.TaskLines.Attach(TaskLines[i]).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        var oldNotes = _context.Notifications.Where(x => x.TaskLineID == TaskLines[i].TaskLineID).ToList() ?? new List<Notification>();
                        _context.Notifications.RemoveRange(oldNotes);
                        await _context.SaveChangesAsync();
                        if (TaskLines[i].WorkerID != "")
                        {

                            Notification note = new Notification()
                            {
                                SenderID = Schedule.ModifiedBy,
                                ToID = TaskLines[i].WorkerID,
                                Title = "Your Schedule was modified by" + User.Identity.Name,
                                Content = "Your Schedule with id : " + TaskLines[i].TaskLineID + " was modified by " + User.Identity.Name,
                                TaskLineID = TaskLines[i].TaskLineID
                            };
                            _context.Notifications.Add(note);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    TaskLines[i].TaskID = Schedule.TaskID;
                    TaskLines[i].TaskLineStatusID = 1;
                    if (TaskLines[i].WorkerID == null)
                    {
                        TaskLines[i].WorkerID = "";
                    }
                    _context.TaskLines.Add(TaskLines[i]);
                    await _context.SaveChangesAsync();

                    Notification note = new Notification()
                    {
                        SenderID = Schedule.CreatedBy,
                        ToID = TaskLines[i].WorkerID,
                        Title = "You have new working schedule created by " + User.Identity.Name,
                        Content = "You have new working line with id : " + TaskLines[i].TaskLineID,
                        TaskLineID = TaskLines[i].TaskLineID
                    };
                    _context.Notifications.Add(note);
                    await _context.SaveChangesAsync();

                }
            }
            for (int i = 0; i < oldLines.Count; i++)
            {
                if (!TaskLines.Any(x => oldLines[i].TaskLineID == x.TaskLineID))
                {
                    var oldNotes = _context.Notifications.Where(x => x.TaskLineID == oldLines[i].TaskLineID);
                    _context.Notifications.RemoveRange(oldNotes);
                    await _context.SaveChangesAsync();

                    _context.Remove(oldLines[i]);
                    await _context.SaveChangesAsync();

                    Notification note = new Notification()
                    {
                        SenderID = Schedule.CreatedBy,
                        ToID = oldLines[i].WorkerID,
                        Title = "Your working schedule was deleted by " + User.Identity.Name,
                        Content = "Your working Schedule for : " + oldLines[i].Day.ToString("MM dd yyyy") + " From : " + oldLines[i].From.ToString("t") + " To : " + oldLines[i].To.ToString("t") + " was deleted",
                        TaskLineID = 0
                    };
                    _context.Notifications.Add(note);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        async public Task<IActionResult> Create(ScheduleView model)
        {
            var Schedule = model.Schedule;
            Schedule.CreatedBy = User.Identity.Name;
            Schedule.ModifiedBy = User.Identity.Name;
            Schedule.DateCreated = DateTime.Now;
            Schedule.DateModified = DateTime.Now;
            _context.Schedules.Add(Schedule);
            await _context.SaveChangesAsync();

            var TaskLines = model.TaskLines;
            for (int i = 0; i < TaskLines.Count; i++)
            {
                TaskLines[i].TaskID = Schedule.TaskID;
                TaskLines[i].TaskLineStatusID = 1;
                if (TaskLines[i].WorkerID == null)
                {
                    TaskLines[i].WorkerID = "";
                }
            }
            _context.TaskLines.AddRange(TaskLines);
            await _context.SaveChangesAsync();

            for (int i = 0; i < TaskLines.Count; i++)
            {
                if (TaskLines[i].WorkerID != "")
                {
                    Notification note = new Notification()
                    {
                        SenderID = Schedule.CreatedBy,
                        ToID = TaskLines[i].WorkerID,
                        Title = "You have new working schedule created by " + User.Identity.Name,
                        Content = "You have new working line with id : " + TaskLines[i].TaskLineID + ". Go into Schedules and check it.",
                        TaskLineID = TaskLines[i].TaskLineID
                    };
                    _context.Notifications.Add(note);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        async public Task<JsonResult> GetWorkers(int role)
        {
            string name = _context.WorkerTypes.SingleOrDefault(x => x.WorkerTypeID == role).WorkerTypeName;
            List<string> response = new List<string>();
            var usersQuery = await _userManager.GetUsersForClaimAsync(new Claim("Manager", User.Identity.Name));
            var users = usersQuery.ToList() ?? new List<IdentityUser>();
            for (int i = 0; i < users.Count; i++)
            {
                var hasRole = await _userManager.IsInRoleAsync(users[i], name);
                if (hasRole)
                {
                    response.Add(users[i].Email);
                }
            }
            return new JsonResult(JArray.FromObject(response));
        }
        [HttpGet]
        async public Task<IActionResult> Details(int id)
        {
            DetailsView detailsView = new DetailsView();
            detailsView.Schedule = await _context.Schedules.SingleOrDefaultAsync(x => x.TaskID == id);
            if (!User.IsInRole("Manager") && !User.IsInRole("Manager"))
            {
                detailsView.TaskLines = _context.TaskLines.Where(x => x.TaskID == id && x.WorkerID == User.Identity.Name).ToList() ?? new List<TaskLine>();
            }
            else
            {
                detailsView.TaskLines = _context.TaskLines.Where(x => x.TaskID == id).ToList() ?? new List<TaskLine>();
            }
            detailsView.WorkerTypes = _context.WorkerTypes.ToList() ?? new List<WorkerType>();
            return View(detailsView);
        }

    }
}
