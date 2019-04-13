using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSchedule.Models.Classes
{
    public class User
    {
        protected string login;
        protected string password;
        protected List<Task> tasks;
        protected List<Task> collaboarations;

        public User() { }
        public User(string login, string password, List<Task> tasks, List<Task> collaboarations)
        {
            this.login = login;
            this.password = password;
            this.tasks = tasks;
            this.collaboarations = collaboarations;
        }

        public List<Task> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
            }
        }
        public List<Task> Collaboarations
        {
            get
            {
                return collaboarations;
            }
            set
            {
                collaboarations = value;
            }
        }
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
    }
}
