using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSchedule.Data;
using WorkSchedule.Models;
using WorkSchedule.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkSchedule.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ScheduleContext _context;

        public NotificationsController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        async public Task<IActionResult> Index(int? filter, bool? inbox)
        {
            List<Notification> Notifications = new List<Notification>();
            int Filter = 1;
            bool Inbox;
            if (filter == 4)
            {
                Filter = 4;
                Notifications = _context.Notifications.Where(x => x.TaskLineID == 0).ToList() ?? new List<Notification>();
            }
            if (inbox == null || inbox == true)
            {
                if (Filter != 4)
                {
                    Notifications = _context.Notifications.Include(x=> x.TaskLine).Where(x => x.ToID == User.Identity.Name).ToList() ?? new List<Notification>();
                }
                else
                {
                    Notifications = Notifications.Where(x => x.ToID == User.Identity.Name).ToList() ?? new List<Notification>();
                }
                Inbox = true;
            }
            else
            {
                if (Filter != 4)
                {
                    Notifications = _context.Notifications.Include(x => x.TaskLine).Where(x => x.SenderID == User.Identity.Name).ToList() ?? new List<Notification>();
                }
                else
                {
                    Notifications = Notifications.Where(x => x.SenderID == User.Identity.Name).ToList() ?? new List<Notification>();
                }
                Inbox = false;
            }
            if (filter == null || filter.Value == 1)
            {
                Notifications = Notifications.Where(x => x.TaskLine.TaskLineStatusID == 1).ToList() ?? new List<Notification>();
                Filter = 1;
            }
            else if (filter == 2)
            {
                Filter = 2;
                Notifications = Notifications.Where(x => x.TaskLine.TaskLineStatusID == 2).ToList() ?? new List<Notification>();
            }
            else if (filter == 3)
            {
                Filter = 3;
                Notifications = Notifications.Where(x => x.TaskLine.TaskLineStatusID == 3).ToList() ?? new List<Notification>();
            }

            NotificationIndexView note = new NotificationIndexView();
            note.Notifications = Notifications;
            note.Inbox = Inbox;
            note.Filter = Filter;
            return View(note);
        }
        [HttpGet]
        async public Task<IActionResult> Show(int id)
        {
            Notification notification = await _context.Notifications.SingleOrDefaultAsync(x => x.NotificationID == id);
            if (notification.TaskLineID != 0)
            {
                notification.TaskLine = await _context.TaskLines.SingleOrDefaultAsync(x => x.TaskLineID == notification.TaskLineID);
                notification.TaskLine.WorkerType = await _context.WorkerTypes.SingleOrDefaultAsync(x => x.WorkerTypeID == notification.TaskLine.WorkerTypeID);
            }
            await _context.SaveChangesAsync();
            return View(notification);
        }
        [HttpPost]
        async public Task<IActionResult> Show(string type, int id)
        {
            var line = await _context.TaskLines.SingleOrDefaultAsync(x => x.TaskLineID == id);
            var task = await _context.Schedules.SingleOrDefaultAsync(x => x.TaskID == line.TaskID);
            if (type == "1")
            {
                line.TaskLineStatusID = 3;
                _context.TaskLines.Attach(line).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else if (type == "2")
            {
                line.TaskLineStatusID = 2;
                _context.TaskLines.Attach(line).State = EntityState.Modified;
                _context.SaveChanges();
                Notification newnote = new Notification()
                {
                    Title = "TaskLine was denied by " + User.Identity.Name,
                    Content = "TaskLine was denied by " + User.Identity.Name + " provide another working time for user or delete TaskLine",
                    SenderID = User.Identity.Name,
                    ToID = task.CreatedBy,
                    TaskLineID = line.TaskLineID
                };
                _context.Notifications.Add(newnote);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
